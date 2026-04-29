using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.RecurringCharges;

public class CreateRecurringChargeRequest
{
    [JsonPropertyName("customer")]
    public string Customer { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public string Price { get; set; } = string.Empty;

    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; }

    [JsonPropertyName("cancelAtPeriodEnd")]
    public bool? CancelAtPeriodEnd { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    [JsonPropertyName("trialPeriodDays")]
    public int? TrialPeriodDays { get; set; }
}
