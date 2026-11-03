using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events.Dispatching;

public sealed class AsyncDispatcherTests
{
    [Fact(DisplayName = "Can be created and publish without subscribers.")]
    [Trait("Category", "Unit")]
    public async Task CanPublishWithoutSubscribersAsync()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        await using var dispatcher = new AsyncDispatcher<IGameEvent>();

        // Act
        var exception = await Record.ExceptionAsync(() =>
            dispatcher.PublishAsync(@event, TestContext.Current.CancellationToken));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Cannot register a null subscriber.")]
    [Trait("Category", "Unit")]
    public async Task CanNotRegisterNullSubscriberAsync()
    {
        // Arrange
        await using var dispatcher = new AsyncDispatcher<IGameEvent>();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Register<GameStartedEvent>(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot unregister a null subscriber.")]
    [Trait("Category", "Unit")]
    public async Task CanNotUnregisterNullSubscriberAsync()
    {
        // Arrange
        await using var dispatcher = new AsyncDispatcher<IGameEvent>();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Unregister<GameStartedEvent>(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot unregister a subscriber for an unregistered event type.")]
    [Trait("Category", "Unit")]
    public async Task CanNotUnregisterUnknownEventTypeAsync()
    {
        // Arrange
        await using var dispatcher = new AsyncDispatcher<IGameEvent>();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Unregister(
                Mock.Of<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Can deliver to matching event types and subscribers in registration order.")]
    [Trait("Category", "Unit")]
    public async Task CanPublishToMatchingSubscribersInOrderAsync()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;

        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var calls = new List<string>();

        var general = new Mock<IAsyncSubscriber<IGameEvent>>(MockBehavior.Strict);
        general.Setup(x => x.OnEventAsync(@event, token))
            .Callback(() => calls.Add("first"))
            .Returns(Task.CompletedTask);

        var first = new Mock<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        first.Setup(x => x.OnEventAsync(@event, token))
            .Callback(() => calls.Add("second"))
            .Returns(Task.CompletedTask);

        var second = new Mock<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        second.Setup(x => x.OnEventAsync(@event, token))
            .Callback(() => calls.Add("third"))
            .Returns(Task.CompletedTask);

        await using var dispatcher = new AsyncDispatcher<IGameEvent>();
        dispatcher.Register(general.Object);
        dispatcher.Register(first.Object);
        dispatcher.Register(Mock.Of<IAsyncSubscriber<TurnEndedEvent>>(MockBehavior.Strict));
        dispatcher.Register(second.Object);

        // Act
        await dispatcher.PublishAsync(@event, token);

        // Assert
        calls.Should().BeEqualTo(["first", "second", "third"]);
    }

    [Fact(DisplayName = "Can await each subscriber before invoking the next one.")]
    [Trait("Category", "Unit")]
    public async Task CanAwaitSubscribersSequentiallyAsync()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;

        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var calls = new List<string>();

        var first = new Mock<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        first.Setup(x => x.OnEventAsync(@event, token))
            .Returns(async () =>
            {
                calls.Add("first started");

                await release.Task.WaitAsync(token);

                calls.Add("first completed");
            });

        var second = new Mock<IAsyncSubscriber<IGameEvent>>(MockBehavior.Strict);
        second.Setup(x => x.OnEventAsync(@event, token))
            .Callback(() => calls.Add("second"))
            .Returns(Task.CompletedTask);

        await using var dispatcher = new AsyncDispatcher<IGameEvent>();
        dispatcher.Register(first.Object);
        dispatcher.Register(second.Object);

        // Act
        var publishing = dispatcher.PublishAsync(@event, token);
        var completedBeforeRelease = publishing.IsCompleted;
        var callsBeforeRelease = calls.ToArray();

        release.SetResult();
        await publishing.WaitAsync(token);

        // Assert
        completedBeforeRelease.Should().BeFalse();
        callsBeforeRelease.Should().Equal("first started");
        calls.Should().Equal("first started", "first completed", "second");
    }

    [Fact(DisplayName = "Can unregister one subscriber while retaining the others.")]
    [Trait("Category", "Unit")]
    public async Task CanUnregisterSubscriberAsync()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;

        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var removed = Mock.Of<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);

        var calls = 0;
        var remaining = new Mock<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        remaining.Setup(x => x.OnEventAsync(@event, token))
            .Callback(() => calls++)
            .Returns(Task.CompletedTask);

        await using var dispatcher = new AsyncDispatcher<IGameEvent>();
        dispatcher.Register(removed);
        dispatcher.Register(remaining.Object);

        // Act
        dispatcher.Unregister(removed);

        await dispatcher.PublishAsync(@event, token);

        // Assert
        calls.Should().Be(1);
    }

    [Fact(DisplayName = "Can unregister the last subscriber and register the event type again.")]
    [Trait("Category", "Unit")]
    public async Task CanRegisterAfterUnregisteringLastSubscriberAsync()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;

        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var removed = Mock.Of<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);

        var calls = 0;
        var replacement = new Mock<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        replacement.Setup(x => x.OnEventAsync(@event, token))
            .Callback(() => calls++)
            .Returns(Task.CompletedTask);

        await using var dispatcher = new AsyncDispatcher<IGameEvent>();
        dispatcher.Register(removed);

        // Act
        dispatcher.Unregister(removed);

        await dispatcher.PublishAsync(@event, token);

        var exception = Record.Exception(() =>
            dispatcher.Unregister(removed));

        dispatcher.Register(replacement.Object);
        await dispatcher.PublishAsync(@event, token);

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();

        calls.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot publish a null event.")]
    [Trait("Category", "Unit")]
    public async Task CanNotPublishNullEventAsync()
    {
        // Arrange
        await using var dispatcher = new AsyncDispatcher<IGameEvent>();

        // Act
        var exception = await Record.ExceptionAsync(() =>
            dispatcher.PublishAsync(null!, TestContext.Current.CancellationToken));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot register a subscriber after disposal.")]
    [Trait("Category", "Unit")]
    public async Task CanNotRegisterAfterDisposeAsync()
    {
        // Arrange
        var dispatcher = new AsyncDispatcher<IGameEvent>();
        await dispatcher.DisposeAsync();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Register(
                Mock.Of<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ObjectDisposedException>();
    }

    [Fact(DisplayName = "Cannot unregister a subscriber after disposal.")]
    [Trait("Category", "Unit")]
    public async Task CanNotUnregisterAfterDisposeAsync()
    {
        // Arrange
        var subscriber = Mock.Of<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);

        var dispatcher = new AsyncDispatcher<IGameEvent>();
        dispatcher.Register(subscriber);

        await dispatcher.DisposeAsync();

        // Act
        var exception = Record.Exception(() =>
            dispatcher.Unregister(subscriber));

        // Assert
        exception.Should().BeOfType<ObjectDisposedException>();
    }

    [Fact(DisplayName = "Cannot publish an event after disposal.")]
    [Trait("Category", "Unit")]
    public async Task CanNotPublishAfterDisposeAsync()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var dispatcher = new AsyncDispatcher<IGameEvent>();
        dispatcher.Register(Mock.Of<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        await dispatcher.DisposeAsync();

        // Act
        var exception = await Record.ExceptionAsync(() =>
            dispatcher.PublishAsync(@event, TestContext.Current.CancellationToken));

        // Assert
        exception.Should().BeOfType<ObjectDisposedException>();
    }

    [Fact(DisplayName = "Can dispose more than once.")]
    [Trait("Category", "Unit")]
    public async Task CanDisposeMoreThanOnceAsync()
    {
        // Arrange
        var dispatcher = new AsyncDispatcher<IGameEvent>();
        dispatcher.Register(Mock.Of<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        await dispatcher.DisposeAsync();

        // Act
        var exception = await Record.ExceptionAsync(async () => await dispatcher.DisposeAsync());

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Can propagate subscriber failures and stop delivery.")]
    [Trait("Category", "Unit")]
    public async Task CanPropagateSubscriberFailureAsync()
    {
        // Arrange
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));
        var token = TestContext.Current.CancellationToken;

        var calls = 0;
        var first = new Mock<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        first.Setup(x => x.OnEventAsync(@event, token))
            .Callback(() => calls++)
            .Returns(Task.FromException(new InvalidOperationException()));

        await using var dispatcher = new AsyncDispatcher<IGameEvent>();
        dispatcher.Register(first.Object);
        dispatcher.Register(Mock.Of<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        // Act
        var exception = await Record.ExceptionAsync(() =>
            dispatcher.PublishAsync(@event, token));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();

        calls.Should().Be(1);
    }

    [Fact(DisplayName = "Can propagate cancellation from a subscriber and stop delivery.")]
    [Trait("Category", "Unit")]
    public async Task CanPropagateSubscriberCancellationAsync()
    {
        // Arrange
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(
            TestContext.Current.CancellationToken);

        var token = cancellation.Token;
        var @event = new GameStartedEvent(new GameId(Guid.NewGuid()));

        var calls = 0;
        var first = new Mock<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict);
        first.Setup(x => x.OnEventAsync(@event, token))
            .Callback(() => calls++)
            .Returns(() => Task.FromCanceled(token));

        await using var dispatcher = new AsyncDispatcher<IGameEvent>();
        dispatcher.Register(first.Object);
        dispatcher.Register(Mock.Of<IAsyncSubscriber<GameStartedEvent>>(MockBehavior.Strict));

        await cancellation.CancelAsync();

        // Act
        var exception = await Record.ExceptionAsync(() =>
            dispatcher.PublishAsync(@event, token));

        // Assert
        exception.Should().BeOfType<TaskCanceledException>();

        calls.Should().Be(1);
    }
}
