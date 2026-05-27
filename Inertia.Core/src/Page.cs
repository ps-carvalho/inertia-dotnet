using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inertia.Core;

public class Page
{
    [JsonPropertyName("component")]
    public string Component { get; set; } = string.Empty;

    [JsonPropertyName("props")]
    public Dictionary<string, object?> Props { get; set; } = new();

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string? Version { get; set; }
}
