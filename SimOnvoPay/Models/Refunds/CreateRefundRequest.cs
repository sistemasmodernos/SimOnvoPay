using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.Refunds;

public class CreateRefundRequest
{
    [JsonPropertyName("paymentIntent")]
    public string PaymentIntent { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public long? Amount { get; set; }

    /// <summary>requested_by_customer | fraudulent | duplicate | other</summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}
