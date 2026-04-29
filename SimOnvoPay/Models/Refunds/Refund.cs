using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.Refunds;

public class Refund
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("paymentIntent")]
    public string PaymentIntent { get; set; } = string.Empty;

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    [JsonPropertyName("liveMode")]
    public bool LiveMode { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }
}
