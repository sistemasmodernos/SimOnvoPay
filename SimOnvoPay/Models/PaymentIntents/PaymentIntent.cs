using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.PaymentIntents;

public class PaymentIntent
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("captureMethod")]
    public string? CaptureMethod { get; set; }

    [JsonPropertyName("customer")]
    public JsonElement? Customer { get; set; }

    [JsonPropertyName("paymentMethod")]
    public JsonElement? PaymentMethod { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("statementDescriptor")]
    public string? StatementDescriptor { get; set; }

    [JsonPropertyName("receiptEmail")]
    public string? ReceiptEmail { get; set; }

    [JsonPropertyName("returnUrl")]
    public string? ReturnUrl { get; set; }

    [JsonPropertyName("nextAction")]
    public JsonElement? NextAction { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    [JsonPropertyName("liveMode")]
    public bool LiveMode { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("amountCaptured")]
    public long? AmountCaptured { get; set; }

    [JsonPropertyName("amountRefunded")]
    public long? AmountRefunded { get; set; }

    [JsonPropertyName("onBehalfOf")]
    public string? OnBehalfOf { get; set; }
}

