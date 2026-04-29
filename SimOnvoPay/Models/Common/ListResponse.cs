using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.Common;

public class ListResponse<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = [];

    [JsonPropertyName("meta")]
    public ListMeta Meta { get; set; } = new();
}
