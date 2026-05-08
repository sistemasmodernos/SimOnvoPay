using System.Text;
using System.Text.Json;
using SimOnvoPay.Api.DTOs;
using SimOnvoPay.Api.Models;

namespace SimOnvoPay.Api.Services;

public class CallbackService(HttpClient httpClient, ILogger<CallbackService> logger) : ICallbackService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task NotifyAsync(PaymentSession session)
    {
        if (string.IsNullOrEmpty(session.CallbackUrl))
            return;

        var payload = new PaymentCompletedCallback
        {
            Token = session.Token,
            Status = session.Status.ToString().ToLowerInvariant(),
            Amount = session.Amount,
            Currency = session.Currency,
            ExternalReference = session.ExternalReference,
            PaymentIntentId = session.OnvoPaymentIntentId,
            PaymentMethodType = session.PaymentMethodType,
            ErrorMessage = session.ErrorMessage,
            CompletedAt = session.CompletedAt ?? DateTime.UtcNow,
            Metadata = DeserializeMetadata(session.MetadataJson)
        };

        var json = JsonSerializer.Serialize(payload, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // 3 intentos con backoff exponencial
        for (var attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                var response = await httpClient.PostAsync(session.CallbackUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation("Callback enviado exitosamente para sesión {Token}", session.Token);
                    return;
                }
                logger.LogWarning("Callback intento {Attempt} falló con status {Status} para sesión {Token}",
                    attempt, (int)response.StatusCode, session.Token);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Callback intento {Attempt} lanzó excepción para sesión {Token}", attempt, session.Token);
            }

            if (attempt < 3)
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
        }

        logger.LogError("Todos los intentos de callback fallaron para sesión {Token} → {Url}", session.Token, session.CallbackUrl);
    }

    private static Dictionary<string, string>? DeserializeMetadata(string? json)
    {
        if (string.IsNullOrEmpty(json)) return null;
        try { return JsonSerializer.Deserialize<Dictionary<string, string>>(json); }
        catch { return null; }
    }
}
