using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events.Dispatching;

public sealed class GameEventDispatcherTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameEventDispatcher(
                Mock.Of<IGameState>(MockBehavior.Strict),
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
                Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Cannot be created without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutGameState()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameEventDispatcher(
                null!,
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
                Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without an event bus writer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutEventBusWriter()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameEventDispatcher(
                Mock.Of<IGameState>(MockBehavior.Strict),
                null!,
                Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without a logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameEventDispatcher(
                Mock.Of<IGameState>(MockBehavior.Strict),
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot register a null producing subscriber.")]
    [Trait("Category", "Unit")]
    public void CanNotRegisterNullProducingSubscriber()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Register((IProducingSubscriber<GameStartedEvent>)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot register a producing subscriber after disposal.")]
    [Trait("Category", "Unit")]
    public void CanNotRegisterProducingSubscriberAfterDispose()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        dispatcher.Dispose();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Register(
                Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ObjectDisposedException>();
    }

    [Fact(DisplayName = "Cannot unregister a null producing subscriber.")]
    [Trait("Category", "Unit")]
    public void CanNotUnregisterNullProducingSubscriber()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Unregister((IProducingSubscriber<GameStartedEvent>)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot unregister a producing subscriber after disposal.")]
    [Trait("Category", "Unit")]
    public void CanNotUnregisterProducingSubscriberAfterDispose()
    {
        // Arrange
        var subscriber = Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict);

        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));
        dispatcher.Register(subscriber);

        dispatcher.Dispose();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Unregister(subscriber));

        // Assert
        exception.Should().BeOfType<ObjectDisposedException>();
    }

    [Fact(DisplayName = "Cannot unregister a producing subscriber for an unregistered event type.")]
    [Trait("Category", "Unit")]
    public void CanNotUnregisterUnknownProducingEventType()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Unregister(
                Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Can deliver to matching producing event types and subscribers in registration order.")]
    [Trait("Category", "Unit")]
    public void CanPublishToProducingSubscribersInOrder()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var writer = Mock.Of<IGameEventBusWriter>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        var calls = new List<string>();

        var general = new Mock<IProducingSubscriber<IGameEvent>>(MockBehavior.Strict);
        general.Setup(x => x.OnEvent(@event, gameState, writer))
            .Callback(() => calls.Add("general"));

        var first = new Mock<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        first.Setup(x => x.OnEvent(@event, gameState, writer))
            .Callback(() => calls.Add("first"));

        var second = new Mock<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        second.Setup(x => x.OnEvent(@event, gameState, writer))
            .Callback(() => calls.Add("second"));

        var dispatcher = new GameEventDispatcher(
            gameState,
            writer,
            logger.Object);
        dispatcher.Register(general.Object);
        dispatcher.Register(first.Object);
        dispatcher.Register(Mock.Of<IProducingSubscriber<TurnEndedEvent>>(MockBehavior.Strict));
        dispatcher.Register(second.Object);
        dispatcher.Register(Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        // Act
        ((IProducingSubcriberDispatcher)dispatcher).Publish(@event);

        // Assert
        calls.Should().Equal("general", "first", "second");
    }

    [Fact(DisplayName = "Can publish without producing subscribers.")]
    [Trait("Category", "Unit")]
    public void CanPublishWithoutProducingSubscribers()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));
        dispatcher.Register(Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            ((IProducingSubcriberDispatcher)dispatcher).Publish(@event));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Can unregister a producing subscriber while retaining the others.")]
    [Trait("Category", "Unit")]
    public void CanUnregisterProducingSubscriber()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var writer = Mock.Of<IGameEventBusWriter>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        var removed = Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict);

        var calls = 0;
        var remaining = new Mock<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        remaining.Setup(x => x.OnEvent(@event, gameState, writer)).Callback(() => calls++);

        var dispatcher = new GameEventDispatcher(
            gameState,
            writer,
            logger.Object);
        dispatcher.Register(removed);
        dispatcher.Register(remaining.Object);

        // Act
        dispatcher.Unregister(removed);

        ((IProducingSubcriberDispatcher)dispatcher).Publish(@event);

        // Assert
        calls.Should().Be(1);
    }

    [Fact(DisplayName = "Can register again after unregistering the last producing subscriber.")]
    [Trait("Category", "Unit")]
    public void CanRegisterAfterUnregisteringLastProducingSubscriber()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var writer = Mock.Of<IGameEventBusWriter>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        var removed = Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict);

        var calls = 0;
        var replacement = new Mock<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        replacement.Setup(x => x.OnEvent(@event, gameState, writer)).Callback(() => calls++);

        var dispatcher = new GameEventDispatcher(
            gameState,
            writer,
            logger.Object);
        dispatcher.Register(removed);

        // Act
        dispatcher.Unregister(removed);

        ((IProducingSubcriberDispatcher)dispatcher).Publish(@event);

        var exception = Record.Exception(() =>
            dispatcher.Unregister(removed));

        dispatcher.Register(replacement.Object);
        ((IProducingSubcriberDispatcher)dispatcher).Publish(@event);

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();

        calls.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot publish a null event to producing subscribers.")]
    [Trait("Category", "Unit")]
    public void CanNotPublishNullEventToProducingSubscribers()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            ((IProducingSubcriberDispatcher)dispatcher).Publish(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot publish to producing subscribers after disposal.")]
    [Trait("Category", "Unit")]
    public void CanNotPublishToProducingSubscribersAfterDispose()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));
        dispatcher.Register(Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        dispatcher.Dispose();

        // Act
        var exception = Record.Exception(() =>
            ((IProducingSubcriberDispatcher)dispatcher).Publish(@event));

        // Assert
        exception.Should().BeOfType<ObjectDisposedException>();
    }

    [Fact(DisplayName = "Can propagate a producing subscriber failure and stop delivery.")]
    [Trait("Category", "Unit")]
    public void CanPropagateProducingSubscriberFailure()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var writer = Mock.Of<IGameEventBusWriter>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        var calls = 0;
        var first = new Mock<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        first.Setup(x => x.OnEvent(@event, gameState, writer))
            .Callback(() => calls++)
            .Throws<InvalidOperationException>();

        var dispatcher = new GameEventDispatcher(
            gameState,
            writer,
            logger.Object);
        dispatcher.Register(first.Object);
        dispatcher.Register(Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            ((IProducingSubcriberDispatcher)dispatcher).Publish(@event));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();

        calls.Should().Be(1);
    }

    [Fact(DisplayName = "Can log producing subscriber delivery when debug logging is enabled.")]
    [Trait("Category", "Unit")]
    public void CanLogProducingSubscriberDelivery()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var writer = Mock.Of<IGameEventBusWriter>(MockBehavior.Strict);

        var calls = 0;
        var subscriber = new Mock<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        subscriber.Setup(x => x.OnEvent(@event, gameState, writer)).Callback(() => calls++);

        var logCalls = 0;
        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
        logger.Setup(x => x.LogDebug(
                "System {SystemType} reacts on event {EventType}",
                It.Is<object[]>(args =>
                    args.Length == 2 &&
                    (string)args[0] == subscriber.Object.GetType().Name &&
                    (string)args[1] == "GameStartedEvent")))
            .Callback(() => logCalls++);

        var dispatcher = new GameEventDispatcher(
            gameState,
            writer,
            logger.Object);
        dispatcher.Register(subscriber.Object);

        // Act
        ((IProducingSubcriberDispatcher)dispatcher).Publish(@event);

        // Assert
        calls.Should().Be(1);
        logCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot register a null consuming subscriber.")]
    [Trait("Category", "Unit")]
    public void CanNotRegisterNullConsumingSubscriber()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Register<GameStartedEvent>((IConsumingSubscriber<GameStartedEvent>)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot register a consuming subscriber after disposal.")]
    [Trait("Category", "Unit")]
    public void CanNotRegisterConsumingSubscriberAfterDispose()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        dispatcher.Dispose();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Register(
                Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ObjectDisposedException>();
    }

    [Fact(DisplayName = "Cannot unregister a null consuming subscriber.")]
    [Trait("Category", "Unit")]
    public void CanNotUnregisterNullConsumingSubscriber()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Unregister<GameStartedEvent>((IConsumingSubscriber<GameStartedEvent>)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot unregister a consuming subscriber after disposal.")]
    [Trait("Category", "Unit")]
    public void CanNotUnregisterConsumingSubscriberAfterDispose()
    {
        // Arrange
        var subscriber = Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict);

        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));
        dispatcher.Register(subscriber);

        dispatcher.Dispose();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Unregister(subscriber));

        // Assert
        exception.Should().BeOfType<ObjectDisposedException>();
    }

    [Fact(DisplayName = "Cannot unregister a consuming subscriber for an unregistered event type.")]
    [Trait("Category", "Unit")]
    public void CanNotUnregisterUnknownConsumingEventType()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Unregister(
                Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Can deliver to matching consuming event types and subscribers in registration order.")]
    [Trait("Category", "Unit")]
    public void CanPublishToConsumingSubscribersInOrder()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        var calls = new List<string>();

        var general = new Mock<IConsumingSubscriber<IGameEvent>>(MockBehavior.Strict);
        general.Setup(x => x.OnEvent(@event, gameState))
            .Callback(() => calls.Add("general"));

        var first = new Mock<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        first.Setup(x => x.OnEvent(@event, gameState))
            .Callback(() => calls.Add("first"));

        var second = new Mock<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        second.Setup(x => x.OnEvent(@event, gameState))
            .Callback(() => calls.Add("second"));

        var dispatcher = new GameEventDispatcher(
            gameState,
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            logger.Object);
        dispatcher.Register(general.Object);
        dispatcher.Register(first.Object);
        dispatcher.Register(Mock.Of<IConsumingSubscriber<TurnEndedEvent>>(MockBehavior.Strict));
        dispatcher.Register(second.Object);
        dispatcher.Register(Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        // Act
        ((IConsumingSubscriberDispatcher)dispatcher).Publish(@event);

        // Assert
        calls.Should().Equal("general", "first", "second");
    }

    [Fact(DisplayName = "Can publish without consuming subscribers.")]
    [Trait("Category", "Unit")]
    public void CanPublishWithoutConsumingSubscribers()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));
        dispatcher.Register(Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            ((IConsumingSubscriberDispatcher)dispatcher).Publish(@event));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Can unregister a consuming subscriber while retaining the others.")]
    [Trait("Category", "Unit")]
    public void CanUnregisterConsumingSubscriber()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        var removed = Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict);

        var calls = 0;
        var remaining = new Mock<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        remaining.Setup(x => x.OnEvent(@event, gameState)).Callback(() => calls++);

        var dispatcher = new GameEventDispatcher(
            gameState,
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            logger.Object);
        dispatcher.Register(removed);
        dispatcher.Register(remaining.Object);

        // Act
        dispatcher.Unregister(removed);

        ((IConsumingSubscriberDispatcher)dispatcher).Publish(@event);

        // Assert
        calls.Should().Be(1);
    }

    [Fact(DisplayName = "Can register again after unregistering the last consuming subscriber.")]
    [Trait("Category", "Unit")]
    public void CanRegisterAfterUnregisteringLastConsumingSubscriber()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        var removed = Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict);

        var calls = 0;
        var replacement = new Mock<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        replacement.Setup(x => x.OnEvent(@event, gameState)).Callback(() => calls++);

        var dispatcher = new GameEventDispatcher(
            gameState,
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            logger.Object);
        dispatcher.Register(removed);

        // Act
        dispatcher.Unregister(removed);

        ((IConsumingSubscriberDispatcher)dispatcher).Publish(@event);

        var exception = Record.Exception(() =>
            dispatcher.Unregister(removed));

        dispatcher.Register(replacement.Object);
        ((IConsumingSubscriberDispatcher)dispatcher).Publish(@event);

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();

        calls.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot publish a null event to consuming subscribers.")]
    [Trait("Category", "Unit")]
    public void CanNotPublishNullEventToConsumingSubscribers()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            ((IConsumingSubscriberDispatcher)dispatcher).Publish(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot publish to consuming subscribers after disposal.")]
    [Trait("Category", "Unit")]
    public void CanNotPublishToConsumingSubscribersAfterDispose()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));
        dispatcher.Register(Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        dispatcher.Dispose();

        // Act
        var exception = Record.Exception(() =>
            ((IConsumingSubscriberDispatcher)dispatcher).Publish(@event));

        // Assert
        exception.Should().BeOfType<ObjectDisposedException>();
    }

    [Fact(DisplayName = "Can propagate a consuming subscriber failure and stop delivery.")]
    [Trait("Category", "Unit")]
    public void CanPropagateConsumingSubscriberFailure()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        var calls = 0;
        var first = new Mock<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        first.Setup(x => x.OnEvent(@event, gameState))
            .Callback(() => calls++)
            .Throws<InvalidOperationException>();

        var dispatcher = new GameEventDispatcher(
            gameState,
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            logger.Object);
        dispatcher.Register(first.Object);
        dispatcher.Register(Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            ((IConsumingSubscriberDispatcher)dispatcher).Publish(@event));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();

        calls.Should().Be(1);
    }

    [Fact(DisplayName = "Can log consuming subscriber delivery when debug logging is enabled.")]
    [Trait("Category", "Unit")]
    public void CanLogConsumingSubscriberDelivery()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var calls = 0;
        var subscriber = new Mock<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        subscriber.Setup(x => x.OnEvent(@event, gameState)).Callback(() => calls++);

        var logCalls = 0;
        var logger = new Mock<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
        logger.Setup(x => x.LogDebug(
                "System {SystemType} applies {EventType}",
                It.Is<object[]>(args =>
                    args.Length == 2 &&
                    (string)args[0] == subscriber.Object.GetType().Name &&
                    (string)args[1] == "GameStartedEvent")))
            .Callback(() => logCalls++);

        var dispatcher = new GameEventDispatcher(
            gameState,
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            logger.Object);
        dispatcher.Register(subscriber.Object);

        // Act
        ((IConsumingSubscriberDispatcher)dispatcher).Publish(@event);

        // Assert
        calls.Should().Be(1);
        logCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Can dispose more than once.")]
    [Trait("Category", "Unit")]
    public void CanDisposeMoreThanOnce()
    {
        // Arrange
        var dispatcher = new GameEventDispatcher(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventBusWriter>(MockBehavior.Strict),
            Mock.Of<ICoreLogger<GameEventDispatcher>>(MockBehavior.Strict));
        dispatcher.Register(Mock.Of<IProducingSubscriber<GameStartedEvent>>(MockBehavior.Strict));
        dispatcher.Register(Mock.Of<IConsumingSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        dispatcher.Dispose();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Dispose());

        // Assert
        exception.Should().BeNull();
    }
}

