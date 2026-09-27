using Centuriin.CardGame.Core.Common.Events.Dispatching;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events.Dispatching;

public sealed class DispatcherTests
{
    [Fact(DisplayName = "Publish should invoke registered subscriber")]
    [Trait("category", "unit")]
    public void PublishShouldInvokeRegisteredSubscriber()
    {
        // Arrange        
        var invokeCalls = 0;
        var subscriberMock = new Mock<ISubscriber<FakeDerivedEvent>>(MockBehavior.Strict);
        subscriberMock
            .Setup(x => x.OnEvent(It.IsAny<FakeDerivedEvent>()))
            .Callback(() => invokeCalls++);

        var dispatcher = new Dispatcher<FakeEventBase>();
        dispatcher.Register(subscriberMock.Object);

        // Act
        dispatcher.Publish(new FakeDerivedEvent());

        // Assert
        invokeCalls.Should().Be(1);
    }

    [Fact]
    public void UnregisterShouldStopSubscriberInvocations()
    {
        // Arrange
        var @event = new FakeDerivedEvent();

        var invocationCount = 0;
        var subscriberMock = new Mock<ISubscriber<FakeDerivedEvent>>(MockBehavior.Strict);
        subscriberMock
            .Setup(x => x.OnEvent(It.IsAny<FakeDerivedEvent>()))
            .Callback(() => invocationCount++);

        var dispatcher = new Dispatcher<FakeEventBase>();
        dispatcher.Register(subscriberMock.Object);

        // Act
        dispatcher.Publish(@event);

        dispatcher.Unregister(subscriberMock.Object);

        dispatcher.Publish(@event);

        // Assert
        invocationCount.Should().Be(1);
    }

    [Fact]
    public void UnregisterNonExistentSubscriberShouldThrowInvalidOperationException()
    {
        // Arrange
        var subscriber = Mock.Of<ISubscriber<FakeDerivedEvent>>();
        var dispatcher = new Dispatcher<FakeEventBase>();

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
        var sub1Calls = 0;
        var sub1 = new Mock<ISubscriber<FakeDerivedEvent>>(MockBehavior.Strict);
        sub1.Setup(x => x.OnEvent(It.IsAny<FakeDerivedEvent>()))
            .Callback(() => sub1Calls++);

        var sub2Calls = 0;
        var sub2 = new Mock<ISubscriber<FakeDerivedEvent>>(MockBehavior.Strict);
        sub2.Setup(x => x.OnEvent(It.IsAny<FakeDerivedEvent>()))
            .Callback(() => sub2Calls++);

        var dispatcher = new Dispatcher<FakeEventBase>();
        dispatcher.Register(sub1.Object);
        dispatcher.Register(sub2.Object);

        // Act
        dispatcher.Publish(new FakeDerivedEvent());

        // Assert
        sub1Calls.Should().Be(1);
        sub2Calls.Should().Be(1);
    }

    [Fact]
    public void PublishShouldInvokeSubscribersForDerivedEvent()
    {
        // Arrange
        var handlerCalls = 0;
        var sub = new Mock<ISubscriber<FakeEventBase>>(MockBehavior.Strict);
        sub.Setup(x => x.OnEvent(It.IsAny<FakeEventBase>()))
            .Callback(() => handlerCalls++);

        var dispatcher = new Dispatcher<FakeEventBase>();
        dispatcher.Register(sub.Object);

        // Act
        dispatcher.Publish(new FakeDerivedEvent());

        // Assert
        handlerCalls.Should().Be(1);
    }

    [Fact]
    public void PublishShouldInvokeSubscribersForAssignableEvent()
    {
        // Arrange
        var sub1Calls = 0;
        var sub1 = new Mock<ISubscriber<FakeDerivedEvent>>(MockBehavior.Strict);
        sub1.Setup(x => x.OnEvent(It.IsAny<FakeDerivedEvent>()))
            .Callback(() => sub1Calls++);

        var sub2Calls = 0;
        var sub2 = new Mock<ISubscriber<FakeDerivedEvent2>>(MockBehavior.Strict);
        sub2.Setup(x => x.OnEvent(It.IsAny<FakeDerivedEvent2>()))
            .Callback(() => sub2Calls++);

        var dispatcher = new Dispatcher<FakeEventBase>();
        dispatcher.Register(sub1.Object);
        dispatcher.Register(sub2.Object);

        FakeEventBase fakeEvent = new FakeDerivedEvent();

        // Act
        dispatcher.Publish(fakeEvent);

        // Assert
        sub1Calls.Should().Be(1);
        sub2Calls.Should().Be(0);
    }

    public abstract class FakeEventBase;

    public sealed class FakeDerivedEvent : FakeEventBase;
    public sealed class FakeDerivedEvent2 : FakeEventBase;
}
