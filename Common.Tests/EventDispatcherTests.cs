using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.System;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common;

public sealed class EventDispatcherTests
{
    [Fact]
    public async Task PublishShouldInvokeRegisteredHandlerAsync()
    {
        // Arrange        
        var invokeCalls = 0;
        Func<FakeTestEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> handler =
            async (e, ct) =>
            {
                invokeCalls++;
                return [];
            };

        var dispatcher = new EventDispatcher();
        dispatcher.Register(handler);

        // Act
        await dispatcher.PublishAsync(
            new FakeTestEvent(),
            TestContext.Current.CancellationToken);

        // Assert
        invokeCalls.Should().Be(1);
    }

    [Fact]
    public async Task PublishShouldHandleRecursiveEventsAsync()
    {
        // Arrange
        var firstEventHandlerCalls = 0;
        var secondEventHandlerCalls = 0;

        var dispatcher = new EventDispatcher();

        dispatcher.Register<FakeTestEvent>(async (e, ct) =>
        {
            firstEventHandlerCalls++;
            return [new FakeChildEvent()];
        });

        dispatcher.Register<FakeChildEvent>(async (e, ct) =>
        {
            secondEventHandlerCalls++;
            return [];
        });

        // Act
        await dispatcher.PublishAsync(
            new FakeTestEvent(),
            TestContext.Current.CancellationToken);

        // Assert
        firstEventHandlerCalls.Should().Be(1);
        secondEventHandlerCalls.Should().Be(1);
    }

    [Fact]
    public async Task UnregisterShouldStopHandlerInvocationsAsync()
    {
        // Arrange
        var @event = new FakeTestEvent();

        var invocationCount = 0;
        Func<FakeTestEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> handler =
            async (e, ct) => { invocationCount++; return []; };

        var dispatcher = new EventDispatcher();

        dispatcher.Register(handler);

        // Act
        await dispatcher.PublishAsync(
            @event,
            TestContext.Current.CancellationToken);

        dispatcher.Unregister(handler);

        await dispatcher.PublishAsync(
            @event,
            TestContext.Current.CancellationToken);

        // Assert
        invocationCount.Should().Be(1);
    }

    [Fact]
    public void UnregisterNonExistentHandlerShouldThrowInvalidOperationException()
    {
        // Arrange
        Func<FakeTestEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> handler =
            async (e, ct) => [];

        var dispatcher = new EventDispatcher();

        // Act
        var exception = Record.Exception(() => dispatcher.Unregister(handler));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task PublishShouldInvokeMultipleHandlersForSameEventAsync()
    {
        // Arrange
        var handlerCalls = 0;
        Func<FakeTestEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> handler1 =
            async (e, ct) => { handlerCalls++; return []; };
        Func<FakeTestEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> handler2 =
            async (e, ct) => { handlerCalls++; return []; };

        var dispatcher = new EventDispatcher();

        dispatcher.Register(handler1);
        dispatcher.Register(handler2);

        // Act
        await dispatcher.PublishAsync(
            new FakeTestEvent(),
            TestContext.Current.CancellationToken);

        // Assert
        handlerCalls.Should().Be(2);
    }

    [Fact]
    public async Task PublishShouldInvokeHandlersForDerivedEventAsync()
    {
        // Arrange
        var handlerCalls = 0;
        Func<FakeTestEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> h1 =
            async (e, ct) => { handlerCalls++; return []; };

        var dispatcher = new EventDispatcher();

        dispatcher.Register(h1);

        // Act
        await dispatcher.PublishAsync(
            new DerivedTestEvent(),
            TestContext.Current.CancellationToken);

        // Assert
        handlerCalls.Should().Be(1);
    }

    public record FakeTestEvent : ISystemEvent
    {
        public GameId GameId => throw new NotImplementedException();
    }

    public record DerivedTestEvent : FakeTestEvent;

    public record FakeChildEvent : ISystemEvent
    {
        public GameId GameId => throw new NotImplementedException();
    }
}
