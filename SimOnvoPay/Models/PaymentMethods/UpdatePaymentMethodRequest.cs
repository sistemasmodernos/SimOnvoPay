using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.PaymentMethods;

public class UpdatePaymentMethodRequest
{
    [JsonPropertyName("billingDetails")]
    public BillingDetails? BillingDetails { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}
