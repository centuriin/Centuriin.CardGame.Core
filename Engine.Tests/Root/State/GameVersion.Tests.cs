using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Root.State;

public sealed class GameVersionTests
{
    [Theory(DisplayName = "Can create a non-negative game version.")]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [Trait("Category", "Unit")]
    public void CanBeCreated(int value)
    {
        // Act
        var version = new GameVersion(value);

        // Assert
        version.Value.Should().Be(value);
    }

    [Theory(DisplayName = "Cannot create a negative game version.")]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithNegativeValue(int value)
    {
        // Act
        var exception = Record.Exception(() =>
            new GameVersion(value));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Fact(DisplayName = "Can increment a game version without mutating a copied value.")]
    [Trait("Category", "Unit")]
    public void CanIncrementVersion()
    {
        // Arrange
        var version = GameVersion.Default;

        var previous = version;

        // Act
        version++;

        // Assert
        previous.Value.Should().Be(0);
        version.Value.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot increment beyond the maximum supported game version.")]
    [Trait("Category", "Unit")]
    public void CanNotIncrementBeyondMaximumVersion()
    {
        // Arrange
        var version = new GameVersion(int.MaxValue);

        // Act
        var exception = Record.Exception(() =>
            version++);

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
        version.Value.Should().Be(int.MaxValue);
    }

    [Theory(DisplayName = "Can compare game versions by their numeric values.")]
    [InlineData(1, 2, -1)]
    [InlineData(2, 2, 0)]
    [InlineData(3, 2, 1)]
    [Trait("Category", "Unit")]
    public void CanCompareVersions(int firstValue, int secondValue, int expected)
    {
        // Arrange
        var first = new GameVersion(firstValue);
        var second = new GameVersion(secondValue);

        // Act
        var comparison = first.CompareTo(second);

        // Assert
        Math.Sign(comparison).Should().Be(expected);
        (first > second).Should().Be(expected > 0);
        (first >= second).Should().Be(expected >= 0);
        (first < second).Should().Be(expected < 0);
        (first <= second).Should().Be(expected <= 0);
    }
}
