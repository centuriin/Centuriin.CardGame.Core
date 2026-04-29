using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Configuration;

public sealed class GamePipelineConfiguratorTests
{
    [Fact]
    public void SetupShouldCreateAndRegisterSystemsInCorrectOrder()
    {
        // Arrange
        var registrationOrder = new List<SystemBase>();
        var factoryMock = new Mock<ISystemFactory>(MockBehavior.Strict);
        var dispatcherMock = new Mock<IEventDispatcher>(MockBehavior.Strict);

        var systemA = new FakeSystemA();
        var systemB = new FakeSystemB();

        factoryMock
            .Setup(x => x.Create(typeof(FakeSystemA)))
            .Returns(systemA);

        factoryMock
            .Setup(x => x.Create(typeof(FakeSystemB)))
            .Returns(systemB);

        dispatcherMock
            .Setup(x => x.Register(It.IsAny<FakeSystemA>()))
            .Callback<ISubscriber<FakeEventA>>(s => registrationOrder.Add((SystemBase)s));

        dispatcherMock
            .Setup(x => x.Register(It.IsAny<FakeSystemB>()))
            .Callback<ISubscriber<FakeEventB>>(s => registrationOrder.Add((SystemBase)s));

        var configurator = new GamePipelineConfigurator(
            factoryMock.Object,
            dispatcherMock.Object);

        configurator.Add<FakeSystemA, FakeEventA>();
        configurator.Add<FakeSystemB, FakeEventB>();

        // Act
        configurator.Setup();

        // Assert
        registrationOrder.Should().HaveCount(2);
        registrationOrder[0].Should().Be(systemA);
        registrationOrder[1].Should().Be(systemB);
    }

    [Fact]
    public void AddAfterShouldInsertSystemAtCorrectPosition()
    {
        // Arrange
        var registrationOrder = new List<SystemBase>();

        var systemA = new FakeSystemA();
        var systemB = new FakeSystemB();
        var systemC = new FakeSystemC();

        var factoryMock = new Mock<ISystemFactory>(MockBehavior.Strict);
        factoryMock.Setup(x => x.Create(typeof(FakeSystemA))).Returns(systemA);
        factoryMock.Setup(x => x.Create(typeof(FakeSystemB))).Returns(systemB);
        factoryMock.Setup(x => x.Create(typeof(FakeSystemC))).Returns(systemC);

        var dispatcherMock = new Mock<IEventDispatcher>(MockBehavior.Strict);
        dispatcherMock
            .Setup(x => x.Register(It.IsAny<FakeSystemA>()))
            .Callback<ISubscriber<FakeEventA>>(s => registrationOrder.Add((SystemBase)s));

        dispatcherMock
            .Setup(x => x.Register(It.IsAny<FakeSystemC>()))
            .Callback<ISubscriber<FakeEventA>>(s => registrationOrder.Add((SystemBase)s));

        dispatcherMock
            .Setup(x => x.Register(It.IsAny<FakeSystemB>()))
            .Callback<ISubscriber<FakeEventB>>(s => registrationOrder.Add((SystemBase)s));

        var configurator = new GamePipelineConfigurator(
            factoryMock.Object,
            dispatcherMock.Object);

        configurator.Add<FakeSystemA, FakeEventA>();
        configurator.Add<FakeSystemC, FakeEventA>();

        // Act
        configurator.AddAfter<FakeSystemA, FakeSystemB, FakeEventB>();
        configurator.Setup();

        // Assert
        registrationOrder.Should().SatisfyRespectively(
            first => first.Should().Be(systemA),
            second => second.Should().Be(systemB),
            third => third.Should().Be(systemC));
    }

    [Fact]
    public void ReplaceShouldOverrideExistingSystemRegistration()
    {
        // Arrange
        var newSystem = new FakeSystemC();

        var factoryMock = new Mock<ISystemFactory>(MockBehavior.Strict);
        factoryMock
            .Setup(x => x.Create(typeof(FakeSystemC)))
            .Returns(newSystem);

        var registrationCount = 0;
        var dispatcherMock = new Mock<IEventDispatcher>(MockBehavior.Strict);
        dispatcherMock
            .Setup(x => x.Register(newSystem))
            .Callback(() => registrationCount++);

        var configurator = new GamePipelineConfigurator(
            factoryMock.Object,
            dispatcherMock.Object);

        configurator.Add<FakeSystemA, FakeEventA>();
        configurator.Replace<FakeSystemA, FakeSystemC, FakeEventA>();

        // Act
        configurator.Setup();

        // Assert
        registrationCount.Should().Be(1);
    }

    [Fact]
    public void SetupShouldThrowWhenTargetSystemIsMissingForAddBefore()
    {
        // Arrange
        var configurator = new GamePipelineConfigurator(
            Mock.Of<ISystemFactory>(MockBehavior.Strict),
            Mock.Of<IEventDispatcher>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            configurator.AddBefore<FakeSystemA, FakeSystemB, FakeEventB>());

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public void AddShouldThrowExceptionIfAlreadyInitialized()
    {
        // Arrange
        var factoryMock = new Mock<ISystemFactory>(MockBehavior.Strict);
        factoryMock
            .Setup(x => x.Create(It.IsAny<Type>()))
            .Returns(new FakeSystemA());

        var dispatcherMock = new Mock<IEventDispatcher>(MockBehavior.Strict);
        dispatcherMock
            .Setup(x => x.Register(It.IsAny<FakeSystemA>()));

        var configurator = new GamePipelineConfigurator(
            factoryMock.Object,
            dispatcherMock.Object);

        configurator.Setup();

        // Act
        var exception = Record.Exception(() =>
            configurator.Add<FakeSystemB, FakeEventB>());

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<InvalidOperationException>();
    }

    public abstract class FakeSystemBase : SystemBase
    {
        protected FakeSystemBase() : 
            base(Mock.Of<IGameState>(), Mock.Of<IGameEventBusWriter>(), Mock.Of<ICoreLogger>())
        {
        }
    }

    public sealed class FakeSystemA : FakeSystemBase, ISubscriber<FakeEventA>
    {
        public void OnEvent(FakeEventA e) { }
    }

    public sealed class FakeSystemB : FakeSystemBase, ISubscriber<FakeEventB>
    {
        public void OnEvent(FakeEventB e) { }
    }

    public sealed class FakeSystemC : FakeSystemBase, ISubscriber<FakeEventA>
    {
        public void OnEvent(FakeEventA e) { }
    }

    public sealed record FakeEventA : IGameEvent { public GameId GameId => default; }
    public sealed record FakeEventB : IGameEvent { public GameId GameId => default; }
}