using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Systems;

public sealed class TurnFlowSystemTests
{
    [Fact]
    public void OnTurnFlowDefinedShouldSetCycleAndStartTurnWhenIsCycledTrue()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var players = new[] { new EntityId(1), new EntityId(2) };

        var setCycleCalls = 0;
        var automatMock = new Mock<ITurnAutomat>(MockBehavior.Strict);
        automatMock.Setup(x => x.SetCycle(players))
            .Callback(() => setCycleCalls++);
        automatMock.SetupGet(x => x.ActivePlayer)
            .Returns(players[0]);

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.SetupGet(x => x.TurnAutomat)
            .Returns(automatMock.Object);

        var eventsList = new List<IGameEvent>();
        var writer = new Mock<IGameEventBus>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<IGameEvent>()))
            .Callback((IGameEvent e) => eventsList.Add(e));

        var system = new TurnFlowSystem(
            stateMock.Object,
            writer.Object,
            Mock.Of<ICoreLogger<TurnFlowSystem>>());

        // Act
        var @event = new TurnFlowDefinedEvent(gameId, players, IsCycled: true);
        system.OnEvent(@event, stateMock.Object, writer.Object);

        // Assert
        setCycleCalls.Should().Be(1);
        eventsList.Single().Should().BeOfType<TurnStartedEvent>()
            .Which.PlayerId.Should().Be(players[0]);
    }

    [Fact]
    public void OnTurnFlowDefinedShouldSetNextAndStartTurnWhenIsCycledFalse()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var players = new[] { new EntityId(1) };

        var setNextCalls = 0;
        var automatMock = new Mock<ITurnAutomat>(MockBehavior.Strict);
        automatMock.Setup(x => x.SetNext(players))
            .Callback(() => setNextCalls++);
        automatMock.SetupGet(x => x.ActivePlayer)
            .Returns(players[0]);

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.SetupGet(x => x.TurnAutomat).Returns(automatMock.Object);

        var system = new TurnFlowSystem(
            stateMock.Object,
            Mock.Of<IGameEventBusWriter>(),
            Mock.Of<ICoreLogger<TurnFlowSystem>>());

        // Act
        var @event = new TurnFlowDefinedEvent(gameId, players, IsCycled: false);
        system.OnEvent(@event, stateMock.Object, Mock.Of<IGameEventBus>());

        // Assert
        setNextCalls.Should().Be(1);
    }

    [Fact]
    public void OnTurnEndedShouldMoveNextAndNotifyNextPlayer()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var p1 = new EntityId(1);
        var p2 = new EntityId(2);

        var moveNextCalls = 0;
        var automatMock = new Mock<ITurnAutomat>(MockBehavior.Strict);
        automatMock.Setup(x => x.MoveNext())
            .Callback(() => moveNextCalls++);
        automatMock.SetupGet(x => x.ActivePlayer)
            .Returns(p2);

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.SetupGet(x => x.TurnAutomat).Returns(automatMock.Object);

        var eventsList = new List<IGameEvent>();
        var writer = new Mock<IGameEventBus>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<IGameEvent>()))
            .Callback((IGameEvent e) => eventsList.Add(e));

        var system = new TurnFlowSystem(
            stateMock.Object,
            writer.Object,
            Mock.Of<ICoreLogger<TurnFlowSystem>>());

        // Act
        var @event = new TurnEndedEvent(gameId, p1);
        system.OnEvent(@event, stateMock.Object, writer.Object);

        // Assert
        moveNextCalls.Should().Be(1);

        eventsList.Single().Should().BeOfType<TurnStartedEvent>()
            .Which.PlayerId.Should().Be(p2);
    }
}