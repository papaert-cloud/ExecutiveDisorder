using ExecutiveDisorder.Core.Models;
using Xunit;

namespace ExecutiveDisorder.Tests;

public class GameResourcesTests
{
    [Fact]
    public void Constructor_ClampsValues_ToValidRange()
    {
        // Arrange & Act
        var resources1 = new GameResources(150, -50, 75, 200);

        // Assert
        Assert.Equal(100, resources1.Popularity); // Clamped from 150
        Assert.Equal(0, resources1.Stability);     // Clamped from -50
        Assert.Equal(75, resources1.MediaTrust);   // Within range
        Assert.Equal(100, resources1.Economic);    // Clamped from 200
    }

    [Fact]
    public void ApplyEffects_UpdatesResources_Correctly()
    {
        // Arrange
        var resources = new GameResources(50, 50, 50, 50);

        // Act
        resources.ApplyEffects(10, -15, 20, -5);

        // Assert
        Assert.Equal(60, resources.Popularity);
        Assert.Equal(35, resources.Stability);
        Assert.Equal(70, resources.MediaTrust);
        Assert.Equal(45, resources.Economic);
    }

    [Fact]
    public void ApplyEffects_ClampsResults_ToValidRange()
    {
        // Arrange
        var resources = new GameResources(90, 10, 50, 50);

        // Act
        resources.ApplyEffects(20, -15, 0, 0);

        // Assert
        Assert.Equal(100, resources.Popularity); // Clamped from 110
        Assert.Equal(0, resources.Stability);     // Clamped from -5
    }

    [Theory]
    [InlineData(0, 50, 50, 50, true)]  // Popularity = 0
    [InlineData(50, 0, 50, 50, true)]  // Stability = 0
    [InlineData(50, 50, 0, 50, true)]  // MediaTrust = 0
    [InlineData(50, 50, 50, 0, true)]  // Economic = 0
    [InlineData(1, 1, 1, 1, false)]    // All > 0
    [InlineData(100, 100, 100, 100, false)] // All at max
    public void IsGameOver_ReturnsCorrectly_BasedOnResources(
        int pop, int stab, int media, int econ, bool expectedGameOver)
    {
        // Arrange
        var resources = new GameResources(pop, stab, media, econ);

        // Act
        var isGameOver = resources.IsGameOver();

        // Assert
        Assert.Equal(expectedGameOver, isGameOver);
    }

    [Fact]
    public void MultipleEffects_MaintainClampingBehavior()
    {
        // Arrange
        var resources = new GameResources(50, 50, 50, 50);

        // Act - Apply multiple effects
        resources.ApplyEffects(30, 30, 30, 30);  // 80, 80, 80, 80
        resources.ApplyEffects(30, 30, 30, 30);  // Should clamp to 100

        // Assert
        Assert.Equal(100, resources.Popularity);
        Assert.Equal(100, resources.Stability);
        Assert.Equal(100, resources.MediaTrust);
        Assert.Equal(100, resources.Economic);
    }

    [Fact]
    public void NegativeEffects_CanTriggerGameOver()
    {
        // Arrange
        var resources = new GameResources(10, 50, 50, 50);

        // Act
        resources.ApplyEffects(-15, 0, 0, 0);

        // Assert
        Assert.True(resources.IsGameOver());
        Assert.Equal(0, resources.Popularity); // Clamped from -5
    }

    [Fact]
    public void CumulativeUnderflow_ClampsToZeroAndTriggersGameOver()
    {
        // Arrange
        var resources = new GameResources(30, 30, 30, 30);

        // Act - Apply cumulative negative effects
        resources.ApplyEffects(-10, -5, -15, -8);   // 20, 25, 15, 22
        resources.ApplyEffects(-10, -10, -10, -10); // 10, 15, 5, 12
        resources.ApplyEffects(-15, -5, -10, -5);   // 0 (underflow), 10, 0 (underflow), 7

        // Assert
        Assert.True(resources.IsGameOver());
        Assert.Equal(0, resources.Popularity);
        Assert.Equal(0, resources.MediaTrust);
        Assert.Equal(10, resources.Stability); // Still above 0
        Assert.Equal(7, resources.Economic);   // Still above 0
    }

    [Fact]
    public void ZeroEffects_DoesNotChangeResources()
    {
        // Arrange
        var resources = new GameResources(50, 60, 70, 80);

        // Act
        resources.ApplyEffects(0, 0, 0, 0);

        // Assert
        Assert.Equal(50, resources.Popularity);
        Assert.Equal(60, resources.Stability);
        Assert.Equal(70, resources.MediaTrust);
        Assert.Equal(80, resources.Economic);
        Assert.False(resources.IsGameOver());
    }
}
