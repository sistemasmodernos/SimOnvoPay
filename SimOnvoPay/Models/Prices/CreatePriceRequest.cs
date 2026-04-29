using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.Prices;

public class CreatePriceRequest
{
    [JsonPropertyName("product")]
    public string Product { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "CRC";

    [JsonPropertyName("unitAmount")]
    public long UnitAmount { get; set; }

    /// <summary>one_time | recurring</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "one_time";

    [JsonPropertyName("recurring")]
    public RecurringInterval? Recurring { get; set; }

    [JsonPropertyName("active")]
    public bool? Active { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}
