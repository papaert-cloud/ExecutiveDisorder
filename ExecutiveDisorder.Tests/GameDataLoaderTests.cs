using ExecutiveDisorder.Core.Services;
using System.IO;
using System.Text.Json;
using Xunit;

namespace ExecutiveDisorder.Tests;

public class GameDataLoaderTests
{
    private readonly string tempDataDir;

    public GameDataLoaderTests()
    {
        tempDataDir = Path.Combine(Path.GetTempPath(), "ExecutiveDisorderTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDataDir);
    }

    [Fact]
    public void LoadCharacters_WithValidJson_ReturnsCharacterList()
    {
        // Arrange
        var json = @"{
            ""characters"": [
                {
                    ""id"": 1,
                    ""name"": ""Test Character"",
                    ""archetypeName"": ""The Tester"",
                    ""description"": ""A test character"",
                    ""startingPopularity"": 50,
                    ""startingStability"": 60,
                    ""startingMediaTrust"": 70,
                    ""startingEconomic"": 80,
                    ""bonuses"": [""Bonus 1"", ""Bonus 2""]
                }
            ]
        }";

        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);
        var filePath = Path.Combine(dataDir, "charactersjson.json");
        File.WriteAllText(filePath, json);

        try
        {
            // Act
            var result = GameDataLoader.LoadCharacters();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Characters);
            Assert.Equal("Test Character", result.Characters[0].Name);
            Assert.Equal(50, result.Characters[0].StartingPopularity);
        }
        finally
        {
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }

    [Fact]
    public void LoadCards_WithValidJson_ReturnsDecisionCardList()
    {
        // Arrange
        var json = @"{
            ""cards"": [
                {
                    ""id"": 1,
                    ""situation"": ""Test situation"",
                    ""choices"": [
                        {
                            ""choiceText"": ""Option 1"",
                            ""popularityEffect"": 10,
                            ""stabilityEffect"": -5,
                            ""mediaTrustEffect"": 0,
                            ""economicEffect"": 5,
                            ""outcomes"": [""Outcome 1""]
                        }
                    ],
                    ""mediaReactions"": []
                }
            ]
        }";

        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);
        var filePath = Path.Combine(dataDir, "cardsjson.json");
        File.WriteAllText(filePath, json);

        try
        {
            // Act
            var result = GameDataLoader.LoadCards();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Cards);
            Assert.Equal("Test situation", result.Cards[0].Situation);
            Assert.Single(result.Cards[0].Choices);
            Assert.Equal(10, result.Cards[0].Choices[0].PopularityEffect);
        }
        finally
        {
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }

    [Fact]
    public void LoadEndings_WithValidJson_ReturnsEndingList()
    {
        // Arrange
        var json = @"{
            ""endings"": [
                {
                    ""id"": 1,
                    ""title"": ""Test Ending"",
                    ""description"": ""A test ending"",
                    ""resourceRequirements"": {
                        ""popularity"": "">80"",
                        ""stability"": "">70"",
                        ""mediaTrust"": """",
                        ""economic"": """"
                    },
                    ""consequences"": [""Consequence 1""]
                }
            ]
        }";

        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);
        var filePath = Path.Combine(dataDir, "endingjson.json");
        File.WriteAllText(filePath, json);

        try
        {
            // Act
            var result = GameDataLoader.LoadEndings();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Endings);
            Assert.Equal("Test Ending", result.Endings[0].Title);
            Assert.Equal(">80", result.Endings[0].ResourceRequirements.Popularity);
        }
        finally
        {
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }

    [Fact]
    public void LoadCharacters_WithMissingFile_ThrowsFileNotFoundException()
    {
        // Arrange - Ensure no Data directory exists
        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        if (Directory.Exists(dataDir))
        {
            var charFile = Path.Combine(dataDir, "charactersjson.json");
            if (File.Exists(charFile)) File.Delete(charFile);
        }

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => GameDataLoader.LoadCharacters());
    }

    [Fact]
    public void LoadCharacters_WithMalformedJson_ThrowsJsonException()
    {
        // Arrange
        var malformedJson = @"{ ""characters"": [ { ""id"": 1, ""name"": ""Test"" } ] }"; // Missing required fields

        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);
        var filePath = Path.Combine(dataDir, "charactersjson.json");
        File.WriteAllText(filePath, "{invalid json}");

        try
        {
            // Act & Assert
            Assert.Throws<JsonException>(() => GameDataLoader.LoadCharacters());
        }
        finally
        {
            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }

    [Fact]
    public void LoadCards_WithEmptyChoicesArray_ReturnsEmptyChoices()
    {
        // Arrange
        var json = @"{
            ""cards"": [
                {
                    ""id"": 1,
                    ""situation"": ""Test"",
                    ""choices"": [],
                    ""mediaReactions"": []
                }
            ]
        }";

        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDir);
        var filePath = Path.Combine(dataDir, "cardsjson.json");
        File.WriteAllText(filePath, json);

        try
        {
            // Act
            var result = GameDataLoader.LoadCards();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Cards);
            Assert.Empty(result.Cards[0].Choices);
        }
        finally
        {
            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }
}
