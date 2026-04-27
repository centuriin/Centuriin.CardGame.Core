using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests;

public sealed class EventDispatcherTests
{
    [Fact]
    public void PublishShouldInvokeRegisteredSubscriber()
    {
        // Arrange        
        var invokeCalls = 0;
        var subscriberMock = new Mock<ISubscriber<FakeTestEvent>>(MockBehavior.Strict);
        subscriberMock
            .Setup(x => x.OnEvent(It.IsAny<FakeTestEvent>()))
            .Callback(() => invokeCalls++);

        var dispatcher = new EventDispatcher();
        dispatcher.Register(subscriberMock.Object);

        // Act
        dispatcher.Publish(new FakeTestEvent());

        // Assert
        invokeCalls.Should().Be(1);
    }

    [Fact]
    public void UnregisterShouldStopSubscriberInvocations()
    {
        // Arrange
        var @event = new FakeTestEvent();
        var invocationCount = 0;

        var subscriberMock = new Mock<ISubscriber<FakeTestEvent>>(MockBehavior.Strict);
        subscriberMock
            .Setup(x => x.OnEvent(It.IsAny<FakeTestEvent>()))
            .Callback(() => invocationCount++);

        var dispatcher = new EventDispatcher();
        dispatcher.Register(subscriberMock.Object);

        // Act
        dispatcher.Publish(@event);
        dispatcher.Unregister(subscriberMock.Object);

        var exception = Record.Exception(() =>
            dispatcher.Publish(@event));

        // Assert
        exception.Should().BeNull();
        invocationCount.Should().Be(1);
    }

    [Fact]
    public void UnregisterNonExistentSubscriberShouldThrowInvalidOperationException()
    {
        // Arrange
        var subscriber = Mock.Of<ISubscriber<FakeTestEvent>>();
        var dispatcher = new EventDispatcher();

        // Act
        var exception = Record.Exception(() => dispatcher.Unregister(subscriber));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public void PublishShouldInvokeMultipleSubscribersForSameEvent()
    {
        // Arrange
        var handlerCalls = 0;
        var sub1 = new Mock<ISubscriber<FakeTestEvent>>(MockBehavior.Strict);
        sub1.Setup(x => x.OnEvent(It.IsAny<FakeTestEvent>()))
            .Callback(() => handlerCalls++);

        var sub2 = new Mock<ISubscriber<FakeTestEvent>>(MockBehavior.Strict);
        sub2.Setup(x => x.OnEvent(It.IsAny<FakeTestEvent>()))
            .Callback(() => handlerCalls++);

        var dispatcher = new EventDispatcher();
        dispatcher.Register(sub1.Object);
        dispatcher.Register(sub2.Object);

        // Act
        dispatcher.Publish(new FakeTestEvent());

        // Assert
        handlerCalls.Should().Be(2);
    }

    [Fact]
    public void PublishShouldInvokeSubscribersForDerivedEvent()
    {
        // Arrange
        var handlerCalls = 0;
        var sub = new Mock<ISubscriber<FakeTestEvent>>(MockBehavior.Strict);
        sub.Setup(x => x.OnEvent(It.IsAny<FakeTestEvent>()))
            .Callback(() => handlerCalls++);

        var dispatcher = new EventDispatcher();
        dispatcher.Register(sub.Object);

        // Act
        dispatcher.Publish(new DerivedTestEvent());

        // Assert
        handlerCalls.Should().Be(1);
    }

    public record class FakeTestEvent : IGameEvent
    {
        public GameId GameId => new(Guid.NewGuid());
    }

    public sealed record class DerivedTestEvent : FakeTestEvent;
}
