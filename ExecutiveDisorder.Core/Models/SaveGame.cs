using System.Text.Json.Serialization;

namespace ExecutiveDisorder.Core.Models;

public class SaveGame
{
    [JsonPropertyName("characterName")]
    public string CharacterName { get; set; } = string.Empty;

    [JsonPropertyName("decisionsCount")]
    public int DecisionsCount { get; set; }

    [JsonPropertyName("resources")]
    public GameResources? Resources { get; set; }

    [JsonPropertyName("decisionLog")]
    public List<string> DecisionLog { get; set; } = new();

    [JsonPropertyName("mediaHeadlines")]
    public List<string> MediaHeadlines { get; set; } = new();

    [JsonPropertyName("usedCardIds")]
    public List<int> UsedCardIds { get; set; } = new();

    [JsonPropertyName("savedAt")]
    public DateTime SavedAt { get; set; } = DateTime.Now;
}
