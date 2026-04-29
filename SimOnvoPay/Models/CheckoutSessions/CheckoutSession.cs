using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.CheckoutSessions;

public class CheckoutSession
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("customer")]
    public string? Customer { get; set; }

    [JsonPropertyName("paymentIntent")]
    public string? PaymentIntent { get; set; }

    [JsonPropertyName("successUrl")]
    public string? SuccessUrl { get; set; }

    [JsonPropertyName("cancelUrl")]
    public string? CancelUrl { get; set; }

    [JsonPropertyName("lineItems")]
    public List<LineItem>? LineItems { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("amountTotal")]
    public long? AmountTotal { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    [JsonPropertyName("expiresAt")]
    public long? ExpiresAt { get; set; }

    [JsonPropertyName("liveMode")]
    public bool LiveMode { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }
}

public class LineItem
{
    [JsonPropertyName("price")]
    public string Price { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; } = 1;
}
