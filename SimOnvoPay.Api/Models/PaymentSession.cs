namespace SimOnvoPay.Api.Models;

public class PaymentSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string Currency { get; set; } = "CRC";
    public string Description { get; set; } = string.Empty;
    public PaymentSessionStatus Status { get; set; } = PaymentSessionStatus.Pending;
    public string? OnvoCustomerId { get; set; }
    public string? OnvoPaymentMethodId { get; set; }
    public string? OnvoPaymentIntentId { get; set; }
    public string CallbackUrl { get; set; } = string.Empty;
    public string SuccessUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;
    public string? ExternalReference { get; set; }
    public string? MetadataJson { get; set; }
    public string? PaymentMethodType { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(1);
    public DateTime? CompletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
