using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.PaymentIntents;

public class CreatePaymentIntentRequest
{
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    /// <summary>USD | CRC | GTQ | NIO | PAB | PEN | MXN | COP | HNL</summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "CRC";

    /// <summary>automatic | manual</summary>
    [JsonPropertyName("captureMethod")]
    public string CaptureMethod { get; set; } = "automatic";

    [JsonPropertyName("customer")]
    public string? Customer { get; set; }

    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("statementDescriptor")]
    public string? StatementDescriptor { get; set; }

    [JsonPropertyName("receiptEmail")]
    public string? ReceiptEmail { get; set; }

    [JsonPropertyName("returnUrl")]
    public string? ReturnUrl { get; set; }

    [JsonPropertyName("confirm")]
    public bool? Confirm { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    [JsonPropertyName("onBehalfOf")]
    public string? OnBehalfOf { get; set; }
}
