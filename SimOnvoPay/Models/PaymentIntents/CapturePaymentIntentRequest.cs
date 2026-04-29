using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.PaymentIntents;

public class CapturePaymentIntentRequest
{
    [JsonPropertyName("amountToCapture")]
    public long? AmountToCapture { get; set; }
}
