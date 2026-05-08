using SimOnvoPay.Api.DTOs;
using SimOnvoPay.Api.Models;

namespace SimOnvoPay.Api.Services;

public interface IPaymentSessionService
{
    Task<CreateSessionResponse> CreateSessionAsync(CreateSessionRequest request, string frontendBaseUrl);
    Task<CheckoutInfoResponse?> GetCheckoutInfoAsync(string token);
    Task<ProcessPaymentResponse> ProcessPaymentAsync(string token, ProcessPaymentRequest request);
    Task<bool> CancelSessionAsync(string token);
    Task<SessionStatusResponse?> GetSessionStatusAsync(string token);
    Task ProcessWebhookAsync(string eventType, string paymentIntentId, string? rawStatus);
}
