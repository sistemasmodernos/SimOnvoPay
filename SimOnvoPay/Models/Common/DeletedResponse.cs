using System.Text.Json.Serialization;

namespace SimOnvoPay.Models.Common;

public class DeletedResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}
