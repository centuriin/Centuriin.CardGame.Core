using System.Collections;

using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events;

public sealed class GameEventsBatchTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var version = new GameVersion(5);

        IGameEvent[] events = [new GameStartedEvent(new GameId(Guid.NewGuid()))];

        // Act
        var batch = new GameEventsBatch(version, events);

        // Assert
        batch.CurrentGameVersion.Should().Be(version);
        batch.Events.Should().Equal(events);
    }

    [Fact(DisplayName = "Cannot be created without events.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutEvents()
    {
        // Arrange
        var version = new GameVersion(5);

        // Act
        var exception = Record.Exception(() =>
            new GameEventsBatch(version, null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can enumerate events in their original order.")]
    [Trait("Category", "Unit")]
    public void CanEnumerateEventsInOrder()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        IGameEvent[] events =
        [
            new GameStartedEvent(gameId),
            new TurnStartedEvent(gameId, new EntityId(1)),
            new TurnEndedEvent(gameId, new EntityId(1))
        ];

        var batch = new GameEventsBatch(new GameVersion(3), events);

        // Act
        var result = batch.ToArray();

        // Assert
        result.Should().Equal(events);
    }

    [Fact(DisplayName = "Can enumerate events through the non-generic interface.")]
    [Trait("Category", "Unit")]
    public void CanEnumerateEventsThroughNonGenericInterface()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        IGameEvent[] events =
        [
            new GameStartedEvent(gameId),
            new TurnStartedEvent(gameId, new EntityId(1))
        ];

        IEnumerable batch = new GameEventsBatch(new GameVersion(2), events);

        var result = new List<IGameEvent>();

        // Act
        foreach (IGameEvent @event in batch)
        {
            result.Add(@event);
        }

        // Assert
        result.Should().Equal(events);
    }

    [Fact(DisplayName = "Can enumerate an empty batch.")]
    [Trait("Category", "Unit")]
    public void CanEnumerateEmptyBatch()
    {
        // Arrange
        var batch = new GameEventsBatch(GameVersion.Default, []);

        // Act
        var result = batch.ToArray();

        // Assert
        result.Should().BeEmpty();
        batch.Events.Should().BeEmpty();
    }
}
