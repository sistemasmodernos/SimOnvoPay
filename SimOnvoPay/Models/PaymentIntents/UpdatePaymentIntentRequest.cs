using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.PaymentIntents;

public class UpdatePaymentIntentRequest
{
    [JsonPropertyName("amount")]
    public long? Amount { get; set; }

    [JsonPropertyName("customer")]
    public string? Customer { get; set; }

    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("receiptEmail")]
    public string? ReceiptEmail { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}
