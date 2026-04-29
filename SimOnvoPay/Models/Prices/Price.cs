using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.Prices;

public class Price
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("product")]
    public string Product { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("unitAmount")]
    public long UnitAmount { get; set; }

    /// <summary>one_time | recurring</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("recurring")]
    public RecurringInterval? Recurring { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    [JsonPropertyName("liveMode")]
    public bool LiveMode { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }
}

public class RecurringInterval
{
    /// <summary>day | week | month | year</summary>
    [JsonPropertyName("interval")]
    public string Interval { get; set; } = string.Empty;

    [JsonPropertyName("intervalCount")]
    public int IntervalCount { get; set; } = 1;
}
