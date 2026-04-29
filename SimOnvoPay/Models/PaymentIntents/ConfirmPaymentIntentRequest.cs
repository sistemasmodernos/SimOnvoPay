using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.PaymentIntents;

public class ConfirmPaymentIntentRequest
{
    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; }

    [JsonPropertyName("returnUrl")]
    public string? ReturnUrl { get; set; }
}
