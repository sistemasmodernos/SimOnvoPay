using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimOnvoPay.Webhooks;

public class OnvoPayWebhookEvent
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("liveMode")]
    public bool LiveMode { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }

    public T? GetData<T>() =>
        Data.Deserialize<T>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
}

public static class WebhookEventTypes
{
    public const string PaymentIntentSucceeded = "payment-intent.succeeded";
    public const string PaymentIntentFailed = "payment-intent.failed";
    public const string PaymentIntentDeferred = "payment-intent.deferred";
    public const string SubscriptionRenewalSucceeded = "subscription.renewal.succeeded";
    public const string SubscriptionRenewalFailed = "subscription.renewal.failed";
    public const string CheckoutSessionSucceeded = "checkout-session.succeeded";
    public const string MobileTransferReceived = "mobile-transfer.received";
}
