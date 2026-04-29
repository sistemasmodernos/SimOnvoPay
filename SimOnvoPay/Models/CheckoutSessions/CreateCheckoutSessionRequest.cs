using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.CheckoutSessions;

public class CreateCheckoutSessionRequest
{
    [JsonPropertyName("successUrl")]
    public string SuccessUrl { get; set; } = string.Empty;

    [JsonPropertyName("cancelUrl")]
    public string? CancelUrl { get; set; }

    [JsonPropertyName("lineItems")]
    public List<LineItem>? LineItems { get; set; }

    [JsonPropertyName("customer")]
    public string? Customer { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    /// <summary>payment | subscription</summary>
    [JsonPropertyName("mode")]
    public string Mode { get; set; } = "payment";

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    [JsonPropertyName("expiresAt")]
    public long? ExpiresAt { get; set; }
}
