using Centuriin.CardGame.Core.Common.Entities.Players;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common;

public sealed class TurnAutomatTests
{
    [Fact]
    public void SetCycleShouldInitializeQueueAndEnableCycling()
    {
        // Arrange
        var p1 = new PlayerId(Guid.NewGuid());
        var p2 = new PlayerId(Guid.NewGuid());
        var automat = new TurnAutomat();

        // Act
        automat.SetCycle([p1, p2]);

        // Assert
        automat.IsCycled.Should().BeTrue();
        automat.ActivePlayer.Should().Be(p1);
        automat.GetEnumarable().Should().ContainInOrder([p1, p2]);
    }

    [Fact]
    public void MoveNextShouldRotatePlayersWhenCycled()
    {
        // Arrange
        var p1 = new PlayerId(Guid.NewGuid());
        var p2 = new PlayerId(Guid.NewGuid());
        var automat = new TurnAutomat();
        automat.SetCycle([p1, p2]);

        // Act & Assert
        automat.ActivePlayer.Should().Be(p1);

        automat.MoveNext();
        automat.ActivePlayer.Should().Be(p2);

        automat.MoveNext();
        automat.ActivePlayer.Should().Be(p1);
    }

    [Fact]
    public void DropCycleShouldDisableCyclingAndClearPlayersList()
    {
        // Arrange
        var p1 = new PlayerId(Guid.NewGuid());
        var automat = new TurnAutomat();
        automat.SetCycle([p1]);

        // Act
        automat.DropCycle();
        automat.MoveNext();

        // Assert
        automat.IsCycled.Should().BeFalse();
        automat.GetEnumarable().Should().BeEmpty();
    }

    [Fact]
    public void SetNextShouldInsertPlayersAfterActivePlayer()
    {
        // Arrange
        var p1 = new PlayerId(Guid.NewGuid());
        var p2 = new PlayerId(Guid.NewGuid());
        var extra = new PlayerId(Guid.NewGuid());
        var automat = new TurnAutomat();
        automat.SetCycle([p1, p2]);

        // Act
        automat.SetNext([extra]);

        // Assert
        var sequence = automat.GetEnumarable().Should().SatisfyRespectively(
            first => first.Should().Be(p1),
            second => second.Should().Be(extra),
            last => last.Should().Be(p2));
    }

    [Fact]
    public void DropQueueAfterShouldRemoveAllFollowingPlayers()
    {
        // Arrange
        var p1 = new PlayerId(Guid.NewGuid());
        var p2 = new PlayerId(Guid.NewGuid());
        var p3 = new PlayerId(Guid.NewGuid());
        var automat = new TurnAutomat();
        automat.SetCycle([p1, p2, p3]);

        // Act
        automat.DropQueueAfter(p1);
        automat.MoveNext();

        // Assert
        automat.ActivePlayer.Should().Be(p1);
    }
}