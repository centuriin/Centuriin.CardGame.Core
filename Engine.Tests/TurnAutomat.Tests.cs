using Centuriin.CardGame.Core.Engine.ECS.Entities;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests;

public sealed class TurnAutomatTests
{
    [Fact(DisplayName = "Can be created with an empty non-cyclic turn queue.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var automat = new TurnAutomat();

        // Assert
        automat.IsCycled.Should().BeFalse();
        automat.GetEnumerable().Should().BeEmpty();
    }

    [Fact(DisplayName = "Cannot read an active player from an empty turn queue.")]
    [Trait("Category", "Unit")]
    public void CanNotReadActivePlayerFromEmptyQueue()
    {
        // Arrange
        var automat = new TurnAutomat();

        // Act
        var exception = Record.Exception(() =>
            _ = automat.ActivePlayer);

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Can repeat the configured player cycle after its last turn.")]
    [Trait("Category", "Unit")]
    public void CanRepeatCycle()
    {
        // Arrange
        var automat = new TurnAutomat();

        EntityId[] players = [new(1), new(2)];
        automat.SetCycle(players);

        // Act
        automat.MoveNext();
        var secondPlayer = automat.ActivePlayer;
        automat.MoveNext();

        // Assert
        secondPlayer.Should().Be(players[1]);
        automat.IsCycled.Should().BeTrue();
        automat.ActivePlayer.Should().Be(players[0]);
        automat.GetEnumerable().Should().Equal(players);
    }

    [Fact(DisplayName = "Can replace the player cycle and discard previously queued turns.")]
    [Trait("Category", "Unit")]
    public void CanReplaceCycle()
    {
        // Arrange
        var automat = new TurnAutomat();
        automat.SetCycle([new(1), new(2)]);
        automat.SetNext(new EntityId(3));

        // Act
        automat.SetCycle([new(4), new(5)]);

        // Assert
        automat.GetEnumerable().Should().Equal(new EntityId(4), new EntityId(5));
    }

    [Fact(DisplayName = "Can isolate the configured cycle from changes to the input collection.")]
    [Trait("Category", "Unit")]
    public void CanCopyCycleOrder()
    {
        // Arrange
        var automat = new TurnAutomat();

        EntityId[] players = [new(1), new(2)];
        automat.SetCycle(players);

        // Act
        players[0] = new(9);
        automat.MoveNext();
        automat.MoveNext();

        // Assert
        automat.ActivePlayer.Should().Be(new EntityId(1));
    }

    [Fact(DisplayName = "Cannot configure a cycle without a player collection.")]
    [Trait("Category", "Unit")]
    public void CanNotSetCycleWithoutPlayers()
    {
        // Arrange
        var automat = new TurnAutomat();

        // Act
        var exception = Record.Exception(() =>
            automat.SetCycle(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can disable cycling while preserving the remaining turns.")]
    [Trait("Category", "Unit")]
    public void CanDropCycle()
    {
        // Arrange
        var automat = new TurnAutomat();
        automat.SetCycle([new(1), new(2)]);

        // Act
        automat.DropCycle();
        var remaining = automat.GetEnumerable().ToArray();
        automat.MoveNext();
        automat.MoveNext();

        // Assert
        remaining.Should().Equal(new EntityId(1), new EntityId(2));
        automat.IsCycled.Should().BeFalse();
        automat.GetEnumerable().Should().BeEmpty();
    }

    [Fact(DisplayName = "Can insert turns after the active player without replacing it.")]
    [Trait("Category", "Unit")]
    public void CanInsertNextTurns()
    {
        // Arrange
        var automat = new TurnAutomat();
        automat.SetCycle([new(1), new(2)]);

        // Act
        automat.SetNext(new EntityId(3), new EntityId(4));

        // Assert
        automat.ActivePlayer.Should().Be(new EntityId(1));
        automat.GetEnumerable().Should().Equal(
            new EntityId(1),
            new EntityId(3),
            new EntityId(4),
            new EntityId(2));
    }

    [Fact(DisplayName = "Can initialize an empty non-cyclic queue with the supplied turns.")]
    [Trait("Category", "Unit")]
    public void CanInitializeQueue()
    {
        // Arrange
        var automat = new TurnAutomat();

        // Act
        automat.SetNext(new EntityId(1), new EntityId(2));
        automat.MoveNext();

        // Assert
        automat.IsCycled.Should().BeFalse();
        automat.ActivePlayer.Should().Be(new EntityId(2));
        automat.GetEnumerable().Should().Equal(new EntityId(2));
    }

    [Fact(DisplayName = "Cannot insert turns without a player collection.")]
    [Trait("Category", "Unit")]
    public void CanNotSetNextWithoutPlayers()
    {
        // Arrange
        var automat = new TurnAutomat();

        // Act
        var exception = Record.Exception(() =>
            automat.SetNext(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot insert an empty sequence of turns.")]
    [Trait("Category", "Unit")]
    public void CanNotSetNextWithEmptyPlayers()
    {
        // Arrange
        var automat = new TurnAutomat();

        // Act
        var exception = Record.Exception(() =>
            automat.SetNext([]));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Can remove queued turns after a player while preserving the cycle.")]
    [Trait("Category", "Unit")]
    public void CanDropQueueAfterPlayer()
    {
        // Arrange
        var automat = new TurnAutomat();
        automat.SetCycle([new(1), new(2), new(3)]);

        // Act
        automat.DropQueueAfter(new(2));
        var remaining = automat.GetEnumerable().ToArray();
        automat.MoveNext();
        automat.MoveNext();

        // Assert
        remaining.Should().Equal(new EntityId(1), new EntityId(2));
        automat.GetEnumerable().Should().Equal(new EntityId(1), new EntityId(2), new EntityId(3));
    }

    [Fact(DisplayName = "Cannot truncate the turn queue after an absent player.")]
    [Trait("Category", "Unit")]
    public void CanNotDropQueueAfterAbsentPlayer()
    {
        // Arrange
        var automat = new TurnAutomat();
        automat.SetNext(new EntityId(1));

        // Act
        var exception = Record.Exception(() =>
            automat.DropQueueAfter(new(2)));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
        automat.GetEnumerable().Should().Equal(new EntityId(1));
    }

    [Fact(DisplayName = "Cannot advance an empty turn queue.")]
    [Trait("Category", "Unit")]
    public void CanNotAdvanceEmptyQueue()
    {
        // Arrange
        var automat = new TurnAutomat();

        // Act
        var exception = Record.Exception(() =>
            automat.MoveNext());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }
}
