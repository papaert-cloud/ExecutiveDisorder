using System.Text.Json.Serialization;

namespace ExecutiveDisorder.Core.Models;

public class ResourceRequirement
{
    [JsonPropertyName("popularity")]
    public string Popularity { get; set; } = string.Empty;

    [JsonPropertyName("stability")]
    public string Stability { get; set; } = string.Empty;

    [JsonPropertyName("mediaTrust")]
    public string MediaTrust { get; set; } = string.Empty;

    [JsonPropertyName("economic")]
    public string Economic { get; set; } = string.Empty;
}

public class Ending
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("resourceRequirements")]
    public ResourceRequirement ResourceRequirements { get; set; } = new();

    [JsonPropertyName("consequences")]
    public List<string> Consequences { get; set; } = new();
}

public class EndingList
{
    [JsonPropertyName("endings")]
    public List<Ending> Endings { get; set; } = new();
}
