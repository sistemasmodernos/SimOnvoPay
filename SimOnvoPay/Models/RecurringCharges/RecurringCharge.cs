using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.RecurringCharges;

public class RecurringCharge
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("customer")]
    public string Customer { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public string Price { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("currentPeriodStart")]
    public long CurrentPeriodStart { get; set; }

    [JsonPropertyName("currentPeriodEnd")]
    public long CurrentPeriodEnd { get; set; }

    [JsonPropertyName("cancelAtPeriodEnd")]
    public bool CancelAtPeriodEnd { get; set; }

    [JsonPropertyName("canceledAt")]
    public long? CanceledAt { get; set; }

    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    [JsonPropertyName("liveMode")]
    public bool LiveMode { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }
}
