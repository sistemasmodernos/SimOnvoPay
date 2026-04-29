using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.PaymentMethods;

public class VerifyPaymentMethodRequest
{
    [JsonPropertyName("amounts")]
    public List<int>? Amounts { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }
}
