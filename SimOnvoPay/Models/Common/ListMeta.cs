using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.Common;

public class ListMeta
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("cursorNext")]
    public string? CursorNext { get; set; }

    [JsonPropertyName("cursorBefore")]
    public string? CursorBefore { get; set; }
}
