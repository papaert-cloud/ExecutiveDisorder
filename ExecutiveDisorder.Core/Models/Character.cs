using System.Text.Json.Serialization;

namespace ExecutiveDisorder.Core.Models;

public class Character
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("archetypeName")]
    public string ArchetypeName { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("startingPopularity")]
    public int StartingPopularity { get; set; }

    [JsonPropertyName("startingStability")]
    public int StartingStability { get; set; }

    [JsonPropertyName("startingMediaTrust")]
    public int StartingMediaTrust { get; set; }

    [JsonPropertyName("startingEconomic")]
    public int StartingEconomic { get; set; }

    [JsonPropertyName("bonuses")]
    public List<string> Bonuses { get; set; } = new();
}

public class CharacterList
{
    [JsonPropertyName("characters")]
    public List<Character> Characters { get; set; } = new();
}
