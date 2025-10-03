using ExecutiveDisorder.Core.Models;
using Xunit;

namespace ExecutiveDisorder.Tests;

public class ModelValidationTests
{
    [Fact]
    public void Character_Properties_SetCorrectly()
    {
        // Arrange & Act
        var character = new Character
        {
            Id = 1,
            Name = "Test Leader",
            ArchetypeName = "The Progressive",
            Description = "A forward-thinking leader",
            StartingPopularity = 60,
            StartingStability = 55,
            StartingMediaTrust = 50,
            StartingEconomic = 65,
            Bonuses = new List<string> { "Bonus 1", "Bonus 2" }
        };

        // Assert
        Assert.Equal(1, character.Id);
        Assert.Equal("Test Leader", character.Name);
        Assert.Equal("The Progressive", character.ArchetypeName);
        Assert.Equal(60, character.StartingPopularity);
        Assert.Equal(2, character.Bonuses.Count);
    }

    [Fact]
    public void Choice_ResourceEffects_AreCorrect()
    {
        // Arrange & Act
        var choice = new Choice
        {
            ChoiceText = "Implement policy",
            PopularityEffect = 10,
            StabilityEffect = -5,
            MediaTrustEffect = 15,
            EconomicEffect = -10,
            Outcomes = new List<string> { "Policy implemented successfully" }
        };

        // Assert
        Assert.Equal("Implement policy", choice.ChoiceText);
        Assert.Equal(10, choice.PopularityEffect);
        Assert.Equal(-5, choice.StabilityEffect);
        Assert.Equal(15, choice.MediaTrustEffect);
        Assert.Equal(-10, choice.EconomicEffect);
        Assert.Single(choice.Outcomes);
    }

    [Fact]
    public void DecisionCard_WithMultipleChoices_IsValid()
    {
        // Arrange & Act
        var card = new DecisionCard
        {
            Id = 1,
            Situation = "A crisis emerges",
            Choices = new List<Choice>
            {
                new Choice { ChoiceText = "Option A", PopularityEffect = 5 },
                new Choice { ChoiceText = "Option B", PopularityEffect = -5 }
            },
            MediaReactions = new List<MediaReaction>()
        };

        // Assert
        Assert.Equal(1, card.Id);
        Assert.Equal("A crisis emerges", card.Situation);
        Assert.Equal(2, card.Choices.Count);
        Assert.Empty(card.MediaReactions);
    }

    [Fact]
    public void Ending_ResourceRequirements_AreParseable()
    {
        // Arrange & Act
        var ending = new Ending
        {
            Id = 1,
            Title = "Victory",
            Description = "You won!",
            ResourceRequirements = new ResourceRequirement
            {
                Popularity = ">80",
                Stability = ">70",
                MediaTrust = ">60",
                Economic = ">75"
            },
            Consequences = new List<string> { "The nation prospers" }
        };

        // Assert
        Assert.Equal("Victory", ending.Title);
        Assert.Equal(">80", ending.ResourceRequirements.Popularity);
        Assert.Equal(">70", ending.ResourceRequirements.Stability);
        Assert.Single(ending.Consequences);
    }

    [Fact]
    public void MediaReaction_Properties_SetCorrectly()
    {
        // Arrange & Act
        var reaction = new MediaReaction
        {
            Outlet = "News Network",
            Reactions = new List<string> { "Breaking news!", "Scandal uncovered!" }
        };

        // Assert
        Assert.Equal("News Network", reaction.Outlet);
        Assert.Equal(2, reaction.Reactions.Count);
        Assert.Contains("Breaking news!", reaction.Reactions);
    }

    [Fact]
    public void CharacterList_CanContainMultipleCharacters()
    {
        // Arrange & Act
        var characterList = new CharacterList
        {
            Characters = new List<Character>
            {
                new Character { Id = 1, Name = "Character 1" },
                new Character { Id = 2, Name = "Character 2" },
                new Character { Id = 3, Name = "Character 3" }
            }
        };

        // Assert
        Assert.Equal(3, characterList.Characters.Count);
        Assert.Equal("Character 1", characterList.Characters[0].Name);
        Assert.Equal("Character 3", characterList.Characters[2].Name);
    }
}
