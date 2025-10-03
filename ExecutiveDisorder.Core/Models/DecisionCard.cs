using System.Text.Json.Serialization;

namespace ExecutiveDisorder.Core.Models;

public class Choice
{
    [JsonPropertyName("choiceText")]
    public string ChoiceText { get; set; } = string.Empty;

    [JsonPropertyName("popularityEffect")]
    public int PopularityEffect { get; set; }

    [JsonPropertyName("stabilityEffect")]
    public int StabilityEffect { get; set; }

    [JsonPropertyName("mediaTrustEffect")]
    public int MediaTrustEffect { get; set; }

    [JsonPropertyName("economicEffect")]
    public int EconomicEffect { get; set; }

    [JsonPropertyName("outcomes")]
    public List<string> Outcomes { get; set; } = new();
}

public class DecisionCard
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("situation")]
    public string Situation { get; set; } = string.Empty;

    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = new();

    [JsonPropertyName("mediaReactions")]
    public List<MediaReaction> MediaReactions { get; set; } = new();
}

public class MediaReaction
{
    [JsonPropertyName("outlet")]
    public string Outlet { get; set; } = string.Empty;

    [JsonPropertyName("reactions")]
    public List<string> Reactions { get; set; } = new();
}

public class DecisionCardList
{
    [JsonPropertyName("cards")]
    public List<DecisionCard> Cards { get; set; } = new();
}
