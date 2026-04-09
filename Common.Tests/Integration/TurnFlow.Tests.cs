using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.SmokeTests;

public sealed class TurnFlowTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task TurnFlowShouldCycleThroughPlayersCorrectlyAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var gameState = new GameState(gameId, new TurnAutomat());

        var p1 = new PlayerId(Guid.NewGuid());
        var player1 = new Player(p1);
        player1.Add(new PlayerRoleComponent(PlayerRole.Participant));
        gameState.AddEntity<Player, PlayerId>(player1);

        var p2 = new PlayerId(Guid.NewGuid());
        var player2 = new Player(p2);
        player2.Add(new PlayerRoleComponent(PlayerRole.Participant));
        gameState.AddEntity<Player, PlayerId>(player2);

        var turnFlowSystem = new TurnFlowSystem(DebugLogger.Instance);

        var dispatcher = new EventDispatcher();
        dispatcher.Register<TurnFlowDefinedEvent>(turnFlowSystem);
        dispatcher.Register<TurnEndedEvent>(turnFlowSystem);

        var eventsList = new List<IGameEvent>();
        var writer = new Mock<IEventBusWriter>(MockBehavior.Strict);
        writer
            .Setup(x => x.Write(It.IsAny<IGameEvent>()))
            .Callback((IGameEvent e) => eventsList.Add(e));

        var flowDefinedEvent = new TurnFlowDefinedEvent(gameId, [p1, p2], IsCycled: true);
        var endTurnP1 = new TurnEndedEvent(gameId, p1);
        var endTurnP2 = new TurnEndedEvent(gameId, p2);

        // Act
        dispatcher.Publish(flowDefinedEvent, gameState, writer.Object);

        var activeAfterDefinedEvent = gameState.TurnAutomat.ActivePlayer;

        dispatcher.Publish(endTurnP1, gameState, writer.Object);

        var activeAfterEndTurn1Event = gameState.TurnAutomat.ActivePlayer;

        dispatcher.Publish(endTurnP2, gameState, writer.Object);

        var activeAfterEndTurn2Event = gameState.TurnAutomat.ActivePlayer;

        // Assert
        activeAfterDefinedEvent.Should().Be(p1);
        activeAfterEndTurn1Event.Should().Be(p2);
        activeAfterEndTurn2Event.Should().Be(p1);

        eventsList.Count.Should().Be(3);

        eventsList.Should().SatisfyRespectively(
            first => first.Should().BeOfType<TurnStartedEvent>()
                    .Which.PlayerId.Should().Be(p1),
            second => second.Should().BeOfType<TurnStartedEvent>()
                    .Which.PlayerId.Should().Be(p2),
            last => last.Should().BeOfType<TurnStartedEvent>()
                    .Which.PlayerId.Should().Be(p1));

    }
}