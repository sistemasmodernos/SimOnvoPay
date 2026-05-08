namespace SimOnvoPay.Api.DTOs;

public class ProcessPaymentResponse
{
    public string Status { get; set; } = string.Empty; // succeeded | pending | failed | requires_action
    public string Message { get; set; } = string.Empty;
    public string? RedirectUrl { get; set; }
    public string? PaymentIntentId { get; set; }
}
