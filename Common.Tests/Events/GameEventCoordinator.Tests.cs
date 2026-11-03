using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events;

public sealed class GameEventCoordinatorTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameEventCoordinator(
                Mock.Of<IGameEventBus>(MockBehavior.Strict),
                Mock.Of<IProducingSubcriberDispatcher>(MockBehavior.Strict),
                Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Cannot be created without an event bus.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutEventBus()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameEventCoordinator(
                null!,
                Mock.Of<IProducingSubcriberDispatcher>(MockBehavior.Strict),
                Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without a producing dispatcher.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutProducingDispatcher()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameEventCoordinator(
                Mock.Of<IGameEventBus>(MockBehavior.Strict),
                null!,
                Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without a consuming dispatcher.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutConsumingDispatcher()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameEventCoordinator(
                Mock.Of<IGameEventBus>(MockBehavior.Strict),
                Mock.Of<IProducingSubcriberDispatcher>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can apply an event without producing or reading from the event bus.")]
    [Trait("Category", "Unit")]
    public void CanApplyEvent()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var publishCalls = 0;
        var consuming = new Mock<IConsumingSubscriberDispatcher>(MockBehavior.Strict);
        consuming.Setup(x => x.Publish(@event)).Callback(() => publishCalls++);

        var coordinator = new GameEventCoordinator(
            Mock.Of<IGameEventBus>(MockBehavior.Strict),
            Mock.Of<IProducingSubcriberDispatcher>(MockBehavior.Strict),
            consuming.Object);

        // Act
        coordinator.Apply(@event);

        // Assert
        publishCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot apply a null event.")]
    [Trait("Category", "Unit")]
    public void CanNotApplyNullEvent()
    {
        // Arrange
        var coordinator = new GameEventCoordinator(
            Mock.Of<IGameEventBus>(MockBehavior.Strict),
            Mock.Of<IProducingSubcriberDispatcher>(MockBehavior.Strict),
            Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            coordinator.Apply(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can produce an initial event when no further events are queued.")]
    [Trait("Category", "Unit")]
    public void CanProduceInitialEvent()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var calls = new List<string>();

        var consuming = new Mock<IConsumingSubscriberDispatcher>(MockBehavior.Strict);
        consuming.Setup(x => x.Publish(@event))
            .Callback(() => calls.Add("consume"));

        var producing = new Mock<IProducingSubcriberDispatcher>(MockBehavior.Strict);
        producing.Setup(x => x.Publish(@event))
            .Callback(() => calls.Add("produce"));

        var bus = new Mock<IGameEventBus>(MockBehavior.Strict);
        bus.Setup(x => x.TryRead(out It.Ref<IGameEvent>.IsAny))
            .Callback(() => calls.Add("read"))
            .Returns(false);

        var coordinator = new GameEventCoordinator(
            bus.Object,
            producing.Object,
            consuming.Object);

        // Act
        var result = coordinator.Produce(@event);

        // Assert
        result.Should().Equal(@event);

        calls.Should().Equal("consume", "produce", "read");
    }

    [Fact(DisplayName = "Can apply and produce each event in the order read from the bus.")]
    [Trait("Category", "Unit")]
    public void CanProduceEventChainInOrder()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var initial = new GameStartedEvent(gameId);
        var first = new TurnStartedEvent(gameId, new EntityId(1));
        var second = new TurnStartedEvent(gameId, new EntityId(2));
        var successor = new TurnEndedEvent(gameId, new EntityId(1));

        IGameEvent[] queuedEvents = [first, second, successor];

        var calls = new List<(string Action, IGameEvent? Event)>();

        var readCalls = 0;
        var bus = new Mock<IGameEventBus>(MockBehavior.Strict);
        bus.Setup(x => x.TryRead(out It.Ref<IGameEvent>.IsAny))
            .Returns((out IGameEvent next) =>
            {
                var index = readCalls++;
                next = index < queuedEvents.Length ? queuedEvents[index] : null!;
                calls.Add(("read", next));

                return index < queuedEvents.Length;
            });

        var consuming = new Mock<IConsumingSubscriberDispatcher>(MockBehavior.Strict);
        consuming.Setup(x => x.Publish(It.IsAny<IGameEvent>()))
            .Callback<IGameEvent>(@event => calls.Add(("consume", @event)));

        var producing = new Mock<IProducingSubcriberDispatcher>(MockBehavior.Strict);
        producing.Setup(x => x.Publish(It.IsAny<IGameEvent>()))
            .Callback<IGameEvent>(@event => calls.Add(("produce", @event)));

        var coordinator = new GameEventCoordinator(
            bus.Object,
            producing.Object,
            consuming.Object);

        // Act
        var result = coordinator.Produce(initial);

        // Assert
        result.Should().Equal(initial, first, second, successor);

        calls.Should().Equal(
            ("consume", initial), ("produce", initial), ("read", first),
            ("consume", first), ("produce", first), ("read", second),
            ("consume", second), ("produce", second), ("read", successor),
            ("consume", successor), ("produce", successor), ("read", null));

        readCalls.Should().Be(4);
    }

    [Fact(DisplayName = "Can process events that were queued before production.")]
    [Trait("Category", "Unit")]
    public void CanProduceWithPendingEvents()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var initial = new GameStartedEvent(gameId);

        var pending = new TurnStartedEvent(gameId, new EntityId(1));

        var readCalls = 0;
        var bus = new Mock<IGameEventBus>(MockBehavior.Strict);
        bus.Setup(x => x.TryRead(out It.Ref<IGameEvent>.IsAny))
            .Returns((out IGameEvent next) =>
            {
                readCalls++;
                next = readCalls == 1 ? pending : null!;

                return readCalls == 1;
            });

        var consumingCalls = 0;
        var consuming = new Mock<IConsumingSubscriberDispatcher>(MockBehavior.Strict);
        consuming.Setup(x => x.Publish(It.IsAny<IGameEvent>()))
            .Callback(() => consumingCalls++);

        var producingCalls = 0;
        var producing = new Mock<IProducingSubcriberDispatcher>(MockBehavior.Strict);
        producing.Setup(x => x.Publish(It.IsAny<IGameEvent>()))
            .Callback(() => producingCalls++);

        var coordinator = new GameEventCoordinator(
            bus.Object,
            producing.Object,
            consuming.Object);

        // Act
        var result = coordinator.Produce(initial);

        // Assert
        result.Should().Equal(initial, pending);

        consumingCalls.Should().Be(2);
        producingCalls.Should().Be(2);

        readCalls.Should().Be(2);
    }

    [Fact(DisplayName = "Cannot produce from a null event.")]
    [Trait("Category", "Unit")]
    public void CanNotProduceNullEvent()
    {
        // Arrange
        var coordinator = new GameEventCoordinator(
            Mock.Of<IGameEventBus>(MockBehavior.Strict),
            Mock.Of<IProducingSubcriberDispatcher>(MockBehavior.Strict),
            Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            coordinator.Produce(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can propagate an apply failure.")]
    [Trait("Category", "Unit")]
    public void CanPropagateApplyFailure()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var consuming = new Mock<IConsumingSubscriberDispatcher>(MockBehavior.Strict);
        consuming.Setup(x => x.Publish(@event))
            .Throws<InvalidOperationException>();

        var coordinator = new GameEventCoordinator(
            Mock.Of<IGameEventBus>(MockBehavior.Strict),
            Mock.Of<IProducingSubcriberDispatcher>(MockBehavior.Strict),
            consuming.Object);

        // Act
        var exception = Record.Exception(() =>
            coordinator.Apply(@event));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot produce successors when applying the event fails.")]
    [Trait("Category", "Unit")]
    public void CanNotProduceWhenConsumingFails()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var consuming = new Mock<IConsumingSubscriberDispatcher>(MockBehavior.Strict);
        consuming.Setup(x => x.Publish(@event))
            .Throws<InvalidOperationException>();

        var coordinator = new GameEventCoordinator(
            Mock.Of<IGameEventBus>(MockBehavior.Strict),
            Mock.Of<IProducingSubcriberDispatcher>(MockBehavior.Strict),
            consuming.Object);

        // Act
        var exception = Record.Exception(() =>
            coordinator.Produce(@event));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot read further events when producing fails.")]
    [Trait("Category", "Unit")]
    public void CanNotContinueWhenProducingFails()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var consumingCalls = 0;
        var consuming = new Mock<IConsumingSubscriberDispatcher>(MockBehavior.Strict);
        consuming.Setup(x => x.Publish(@event))
            .Callback(() => consumingCalls++);

        var producing = new Mock<IProducingSubcriberDispatcher>(MockBehavior.Strict);
        producing.Setup(x => x.Publish(@event))
            .Throws<InvalidOperationException>();

        var coordinator = new GameEventCoordinator(
            Mock.Of<IGameEventBus>(MockBehavior.Strict),
            producing.Object,
            consuming.Object);

        // Act
        var exception = Record.Exception(() =>
            coordinator.Produce(@event));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();

        consumingCalls.Should().Be(1);
    }
}
