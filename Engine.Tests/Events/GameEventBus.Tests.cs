using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Domain;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Events;

public sealed class GameEventBusTests
{
    [Fact(DisplayName = "Cannot read from an empty bus.")]
    [Trait("Category", "Unit")]
    public void CanNotReadFromEmptyBus()
    {
        // Arrange
        var bus = new GameEventBus();

        // Act
        var result = bus.TryRead(out var @event);

        // Assert
        result.Should().BeFalse();
        @event.Should().BeNull();
    }

    [Fact(DisplayName = "Can read events in the order they were written.")]
    [Trait("Category", "Unit")]
    public void CanReadEventsInOrder()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var first = new GameStartedEvent(gameId);
        var second = new TurnStartedEvent(gameId, new EntityId(1));

        var bus = new GameEventBus();
        bus.Write(first);
        bus.Write(second);

        // Act
        var firstRead = bus.TryRead(out var firstResult);
        var secondRead = bus.TryRead(out var secondResult);
        var thirdRead = bus.TryRead(out var thirdResult);

        // Assert
        firstRead.Should().BeTrue();
        secondRead.Should().BeTrue();
        thirdRead.Should().BeFalse();

        firstResult.Should().BeSameAs(first);
        secondResult.Should().BeSameAs(second);
        thirdResult.Should().BeNull();
    }

    [Fact(DisplayName = "Can write the same event more than once.")]
    [Trait("Category", "Unit")]
    public void CanWriteSameEventMoreThanOnce()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var bus = new GameEventBus();

        // Act
        bus.Write(@event);
        bus.Write(@event);

        var firstRead = bus.TryRead(out var first);
        var secondRead = bus.TryRead(out var second);
        var thirdRead = bus.TryRead(out _);

        // Assert
        firstRead.Should().BeTrue();
        secondRead.Should().BeTrue();
        thirdRead.Should().BeFalse();
        first.Should().BeSameAs(@event);
        second.Should().BeSameAs(@event);
    }
}
