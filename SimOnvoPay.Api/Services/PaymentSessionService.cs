using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SimOnvoPay.Api.Data;
using SimOnvoPay.Api.DTOs;
using SimOnvoPay.Api.Helpers;
using SimOnvoPay.Api.Models;
using SimOnvoPay.Models.Customers;
using SimOnvoPay.Models.PaymentIntents;
using SimOnvoPay.Models.PaymentMethods;

namespace SimOnvoPay.Api.Services;

public class PaymentSessionService(
    AppDbContext db,
    OnvoPayClient onvo,
    ICallbackService callbackService,
    IConfiguration config,
    IMemoryCache cache,
    ILogger<PaymentSessionService> logger) : IPaymentSessionService
{
    public async Task<CreateSessionResponse> CreateSessionAsync(CreateSessionRequest request, string frontendBaseUrl)
    {
        var token = GenerateToken();
        var session = new PaymentSession
        {
            Token = token,
            Amount = request.Amount,
            Currency = request.Currency.ToUpperInvariant(),
            Description = request.Description,
            CallbackUrl = request.CallbackUrl,
            SuccessUrl = request.SuccessUrl,
            CancelUrl = request.CancelUrl,
            ExternalReference = request.ExternalReference,
            MetadataJson = request.Metadata is not null ? JsonSerializer.Serialize(request.Metadata) : null
        };

        db.PaymentSessions.Add(session);
        await db.SaveChangesAsync();

        return new CreateSessionResponse
        {
            Token = token,
            CheckoutUrl = $"{frontendBaseUrl.TrimEnd('/')}/checkout/{token}",
            ExpiresAt = session.ExpiresAt
        };
    }

    public async Task<CheckoutInfoResponse?> GetCheckoutInfoAsync(string token)
    {
        var session = await db.PaymentSessions.FirstOrDefaultAsync(s => s.Token == token);
        if (session is null) return null;

        if (session.Status == PaymentSessionStatus.Pending && session.ExpiresAt < CrDateTime.Now)
        {
            session.Status = PaymentSessionStatus.Expired;
            session.CompletedAt = CrDateTime.Now;
            session.UpdatedAt = CrDateTime.Now;
            await db.SaveChangesAsync();
            _ = callbackService.NotifyAsync(session);
        }

        return new CheckoutInfoResponse
        {
            Token = session.Token,
            Amount = session.Amount,
            Currency = session.Currency,
            Description = session.Description,
            Status = session.Status.ToString().ToLowerInvariant(),
            ExpiresAt = session.ExpiresAt,
            Metadata = DeserializeMetadata(session.MetadataJson)
        };
    }

    public async Task<ProcessPaymentResponse> ProcessPaymentAsync(string token, ProcessPaymentRequest request)
    {
        var session = await db.PaymentSessions.FirstOrDefaultAsync(s => s.Token == token);
        if (session is null)
            return Fail("Sesión de pago no encontrada.");

        if (session.ExpiresAt < CrDateTime.Now)
            return Fail("La sesión de pago ha expirado.");

        if (session.Status != PaymentSessionStatus.Pending)
            return Fail($"La sesión no puede procesarse en estado '{session.Status}'.");

        session.Status = PaymentSessionStatus.Processing;
        session.PaymentMethodType = request.PaymentMethod;
        session.UpdatedAt = CrDateTime.Now;
        await db.SaveChangesAsync();

        try
        {
            // 1. Crear cliente en OnvoPay
            var customer = await onvo.Customers.CreateAsync(new CreateCustomerRequest
            {
                Name = request.CustomerName,
                Email = request.CustomerEmail,
                Phone = request.CustomerPhone
            });
            session.OnvoCustomerId = customer.Id;
            await db.SaveChangesAsync();

            // 2. Crear método de pago
            var paymentMethod = await CreatePaymentMethodAsync(request, customer.Id);
            session.OnvoPaymentMethodId = paymentMethod.Id;
            await db.SaveChangesAsync();

            // 3. Crear intención de pago
            var intentRequest = new CreatePaymentIntentRequest
            {
                Amount = session.Amount,
                Currency = session.Currency,
                Description = session.Description,
                Metadata = DeserializeMetadata(session.MetadataJson)
            };

            var intent = await onvo.PaymentIntents.CreateAsync(intentRequest);
            session.OnvoPaymentIntentId = intent.Id;
            await db.SaveChangesAsync();

            // 4. Confirmar intención con el método de pago
            var frontendUrl = config["AppSettings:FrontendUrl"]?.TrimEnd('/') ?? "";
            var returnUrl = $"{frontendUrl}/checkout/{session.Token}";
            var isPublicUrl = returnUrl.StartsWith("https://") &&
                              !returnUrl.Contains("localhost") &&
                              !returnUrl.Contains("127.0.0.1");
            var confirmed = await onvo.PaymentIntents.ConfirmAsync(intent.Id, new ConfirmPaymentIntentRequest
            {
                PaymentMethodId = paymentMethod.Id,
                ReturnUrl = isPublicUrl ? returnUrl : null
            });

            // 5. Evaluar resultado
            var redirectUrl = ExtractRedirectUrl(confirmed.NextAction);
            logger.LogInformation("Intent {Id} status={Status} nextAction={NextAction} redirectUrl={RedirectUrl}",
                confirmed.Id, confirmed.Status, confirmed.NextAction?.ToString(), redirectUrl);
            return await HandleIntentStatusAsync(session, confirmed.Status, redirectUrl);
        }
        catch (SimOnvoPay.Exceptions.OnvoPayException ex)
        {
            logger.LogError(ex, "Error OnvoPay al procesar sesión {Token}", token);
            session.Status = PaymentSessionStatus.Failed;
            session.ErrorMessage = ex.ApiMessage ?? ex.Message;
            session.CompletedAt = CrDateTime.Now;
            session.UpdatedAt = CrDateTime.Now;
            await db.SaveChangesAsync();
            _ = callbackService.NotifyAsync(session);
            return Fail(session.ErrorMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inesperado al procesar sesión {Token}", token);
            session.Status = PaymentSessionStatus.Failed;
            session.ErrorMessage = "Error interno al procesar el pago.";
            session.CompletedAt = CrDateTime.Now;
            session.UpdatedAt = CrDateTime.Now;
            await db.SaveChangesAsync();
            _ = callbackService.NotifyAsync(session);
            return Fail(session.ErrorMessage);
        }
    }

    private async Task<SimOnvoPay.Models.PaymentMethods.PaymentMethod> CreatePaymentMethodAsync(
        ProcessPaymentRequest request, string customerId)
    {
        var pmRequest = new CreatePaymentMethodRequest();

        switch (request.PaymentMethod)
        {
            case "card":
                pmRequest.Type = "card";
                pmRequest.Card = new CreateCardDetails
                {
                    Number = request.Card!.Number.Replace(" ", ""),
                    ExpMonth = request.Card.ExpMonth,
                    ExpYear = request.Card.ExpYear,
                    HolderName = request.CustomerName,
                    Cvv = request.Card.Cvc
                };
                break;

            case "sinpe":
                pmRequest.Type = "bank_account";
                pmRequest.BankAccount = new CreateBankAccountDetails
                {
                    AccountNumber = request.Sinpe!.AccountNumber,
                    AccountType = request.Sinpe.AccountType
                };
                break;

            case "sinpe_movil":
                pmRequest.Type = "mobile_number";
                pmRequest.MobileNumber = new CreateMobileNumberDetails
                {
                    Number = request.SinpeMovil!.Phone,
                    IdentificationType = request.SinpeMovil.IdentificationType,
                    Identification = request.SinpeMovil.Identification
                };
                break;

            default:
                throw new InvalidOperationException($"Método de pago no soportado: {request.PaymentMethod}");
        }

        return await onvo.PaymentMethods.CreateAsync(pmRequest);
    }

    private async Task<ProcessPaymentResponse> HandleIntentStatusAsync(
        PaymentSession session, string? intentStatus, string? redirectUrl)
    {
        switch (intentStatus?.ToLowerInvariant())
        {
            case "succeeded":
                session.Status = PaymentSessionStatus.Succeeded;
                session.CompletedAt = CrDateTime.Now;
                session.UpdatedAt = CrDateTime.Now;
                await db.SaveChangesAsync();
                _ = callbackService.NotifyAsync(session);
                return new ProcessPaymentResponse { Status = "succeeded", Message = "Pago procesado exitosamente.", PaymentIntentId = session.OnvoPaymentIntentId };

            case "processing":
            case "pending":
            case "requires_payment_method":
                session.Status = PaymentSessionStatus.Pending;
                session.UpdatedAt = CrDateTime.Now;
                await db.SaveChangesAsync();
                return new ProcessPaymentResponse
                {
                    Status = "pending",
                    Message = session.PaymentMethodType == "sinpe_movil"
                        ? "Recibirá una notificación en su teléfono para aprobar el pago."
                        : "El pago está siendo procesado. Por favor espere.",
                    PaymentIntentId = session.OnvoPaymentIntentId
                };

            case "requires_action":
                session.Status = PaymentSessionStatus.Processing;
                session.UpdatedAt = CrDateTime.Now;
                await db.SaveChangesAsync();
                return new ProcessPaymentResponse
                {
                    Status = "requires_action",
                    Message = "Se requiere verificación adicional.",
                    RedirectUrl = redirectUrl,
                    PaymentIntentId = session.OnvoPaymentIntentId
                };

            default:
                session.Status = PaymentSessionStatus.Failed;
                session.ErrorMessage = $"Estado de pago no esperado: {intentStatus}";
                session.CompletedAt = CrDateTime.Now;
                session.UpdatedAt = CrDateTime.Now;
                await db.SaveChangesAsync();
                _ = callbackService.NotifyAsync(session);
                return Fail(session.ErrorMessage);
        }
    }

    public async Task<bool> CancelSessionAsync(string token)
    {
        var session = await db.PaymentSessions.FirstOrDefaultAsync(s => s.Token == token);
        if (session is null) return false;

        if (session.Status is not (PaymentSessionStatus.Pending or PaymentSessionStatus.Processing))
            return false;

        session.Status = PaymentSessionStatus.Cancelled;
        session.CompletedAt = CrDateTime.Now;
        session.UpdatedAt = CrDateTime.Now;
        await db.SaveChangesAsync();

        _ = callbackService.NotifyAsync(session);
        return true;
    }

    public async Task<SessionStatusResponse?> GetSessionStatusAsync(string token)
    {
        var session = await db.PaymentSessions.FirstOrDefaultAsync(s => s.Token == token);
        if (session is null) return null;

        // If still in-flight, query OnvoPay for the current status (throttled to once per 10s per token)
        var cacheKey = $"onvo_check_{token}";
        if (session.OnvoPaymentIntentId is not null &&
            session.Status is PaymentSessionStatus.Processing or PaymentSessionStatus.Pending &&
            !cache.TryGetValue(cacheKey, out _))
        {
            cache.Set(cacheKey, true, TimeSpan.FromSeconds(10));
            try
            {
                var intent = await onvo.PaymentIntents.GetAsync(session.OnvoPaymentIntentId);
                await HandleIntentStatusAsync(session, intent.Status, null);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "No se pudo consultar el estado del intent {Id}", session.OnvoPaymentIntentId);
            }
        }

        return new SessionStatusResponse
        {
            Token = session.Token,
            Status = session.Status.ToString().ToLowerInvariant(),
            Amount = session.Amount,
            Currency = session.Currency,
            ExternalReference = session.ExternalReference,
            PaymentIntentId = session.OnvoPaymentIntentId,
            PaymentMethodType = session.PaymentMethodType,
            ErrorMessage = session.ErrorMessage,
            CreatedAt = session.CreatedAt,
            CompletedAt = session.CompletedAt
        };
    }

    public async Task ProcessWebhookAsync(string eventType, string paymentIntentId, string? rawStatus)
    {
        var session = await db.PaymentSessions.FirstOrDefaultAsync(s => s.OnvoPaymentIntentId == paymentIntentId);
        if (session is null)
        {
            logger.LogWarning("Webhook recibido para PaymentIntent {Id} sin sesión asociada.", paymentIntentId);
            return;
        }

        switch (eventType)
        {
            case "payment-intent.succeeded":
                if (session.Status == PaymentSessionStatus.Succeeded) return;
                session.Status = PaymentSessionStatus.Succeeded;
                session.CompletedAt = CrDateTime.Now;
                break;

            case "payment-intent.failed":
                if (session.Status == PaymentSessionStatus.Failed) return;
                session.Status = PaymentSessionStatus.Failed;
                session.ErrorMessage = "El pago fue rechazado por la entidad bancaria.";
                session.CompletedAt = CrDateTime.Now;
                break;

            default:
                logger.LogInformation("Evento webhook ignorado: {EventType}", eventType);
                return;
        }

        session.UpdatedAt = CrDateTime.Now;
        await db.SaveChangesAsync();

        await callbackService.NotifyAsync(session);
    }

    private static string GenerateToken()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var bytes = RandomNumberGenerator.GetBytes(24);
        var sb = new StringBuilder("pay_");
        foreach (var b in bytes)
            sb.Append(chars[b % chars.Length]);
        return sb.ToString();
    }

    private static string? ExtractRedirectUrl(System.Text.Json.JsonElement? nextAction)
    {
        if (nextAction is not { } el) return null;
        // Try common OnvoPay field paths for the redirect URL
        foreach (var key in new[] { "redirectUrl", "url" })
            if (el.TryGetProperty(key, out var v) && v.ValueKind == System.Text.Json.JsonValueKind.String)
                return v.GetString();
        // nested: redirectToUrl.url
        if (el.TryGetProperty("redirectToUrl", out var nested) &&
            nested.TryGetProperty("url", out var nestedUrl) &&
            nestedUrl.ValueKind == System.Text.Json.JsonValueKind.String)
            return nestedUrl.GetString();
        return null;
    }

    private static ProcessPaymentResponse Fail(string message) =>
        new() { Status = "failed", Message = message };

    private static Dictionary<string, string>? DeserializeMetadata(string? json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        try { return JsonSerializer.Deserialize<Dictionary<string, string>>(json); }
        catch { return null; }
    }
}
