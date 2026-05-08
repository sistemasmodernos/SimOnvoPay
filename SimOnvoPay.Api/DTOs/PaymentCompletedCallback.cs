namespace SimOnvoPay.Api.DTOs;

public class PaymentCompletedCallback
{
    public string Token { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? ExternalReference { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? PaymentMethodType { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }
    public DateTime CompletedAt { get; set; }
}
