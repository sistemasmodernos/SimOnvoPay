using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.PaymentIntents;

public class ConfirmPaymentIntentRequest
{
    [JsonPropertyName("paymentMethodId")]
    public string? PaymentMethodId { get; set; }

    [JsonPropertyName("returnUrl")]
    public string? ReturnUrl { get; set; }
}
