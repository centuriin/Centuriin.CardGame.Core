using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class TurnFlowSystemTests
{
    [Fact]
    public void OnTurnFlowDefinedShouldSetCycleAndStartTurnWhenIsCycledTrue()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var players = new[] { new PlayerId(Guid.NewGuid()), new PlayerId(Guid.NewGuid()) };

        var setCycleCalls = 0;
        var automatMock = new Mock<ITurnAutomat>(MockBehavior.Strict);
        automatMock.Setup(x => x.SetCycle(players))
            .Callback(() => setCycleCalls++);
        automatMock.SetupGet(x => x.ActivePlayer)
            .Returns(players[0]);

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.SetupGet(x => x.TurnAutomat)
            .Returns(automatMock.Object);

        var channel = Channel.CreateUnbounded<IGameEvent>();
        var system = new TurnFlowSystem(Mock.Of<IGameEngineLogger>());

        // Act
        var @event = new TurnFlowDefinedEvent(gameId, players, IsCycled: true);
        system.OnEvent(@event, stateMock.Object, channel.Writer);

        // Assert
        setCycleCalls.Should().Be(1);

        channel.Reader.TryRead(out var produced).Should().BeTrue();
        produced.Should().BeOfType<TurnStartedEvent>()
            .Which.PlayerId.Should().Be(players[0]);
    }

    [Fact]
    public void OnTurnFlowDefinedShouldSetNextAndStartTurnWhenIsCycledFalse()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var players = new[] { new PlayerId(Guid.NewGuid()) };

        var setNextCalls = 0;
        var automatMock = new Mock<ITurnAutomat>(MockBehavior.Strict);
        automatMock.Setup(x => x.SetNext(players))
            .Callback(() => setNextCalls++);
        automatMock.SetupGet(x => x.ActivePlayer)
            .Returns(players[0]);

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.SetupGet(x => x.TurnAutomat).Returns(automatMock.Object);

        var channel = Channel.CreateUnbounded<IGameEvent>();
        var system = new TurnFlowSystem(Mock.Of<IGameEngineLogger>());

        // Act
        var @event = new TurnFlowDefinedEvent(gameId, players, IsCycled: false);
        system.OnEvent(@event, stateMock.Object, channel.Writer);

        // Assert
        setNextCalls.Should().Be(1);
    }

    [Fact]
    public void OnTurnEndedShouldMoveNextAndNotifyNextPlayer()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var p1 = new PlayerId(Guid.NewGuid());
        var p2 = new PlayerId(Guid.NewGuid());

        var moveNextCalls = 0;
        var automatMock = new Mock<ITurnAutomat>(MockBehavior.Strict);
        automatMock.Setup(x => x.MoveNext())
            .Callback(() => moveNextCalls++);
        automatMock.SetupGet(x => x.ActivePlayer)
            .Returns(p2);

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.SetupGet(x => x.TurnAutomat).Returns(automatMock.Object);

        var channel = Channel.CreateUnbounded<IGameEvent>();
        var system = new TurnFlowSystem(Mock.Of<IGameEngineLogger>());

        // Act
        var @event = new TurnEndedEvent(gameId, p1);
        system.OnEvent(@event, stateMock.Object, channel.Writer);

        // Assert
        moveNextCalls.Should().Be(1);

        channel.Reader.TryRead(out var produced).Should().BeTrue();
        produced.Should().BeOfType<TurnStartedEvent>()
            .Which.PlayerId.Should().Be(p2);
    }
}