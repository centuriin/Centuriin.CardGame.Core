using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

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

        var channel = Channel.CreateUnbounded<IGameEvent>();
        var writer = channel.Writer;
        var reader = channel.Reader;

        var flowDefinedEvent = new TurnFlowDefinedEvent(gameId, [p1, p2], IsCycled: true);
        var endTurnP1 = new TurnEndedEvent(gameId, p1);
        var endTurnP2 = new TurnEndedEvent(gameId, p2);

        // Act
        dispatcher.Publish(flowDefinedEvent, gameState, writer);

        var activeAfterDefinedEvent = gameState.TurnAutomat.ActivePlayer;

        dispatcher.Publish(endTurnP1, gameState, writer);

        var activeAfterEndTurn1Event = gameState.TurnAutomat.ActivePlayer;

        dispatcher.Publish(endTurnP2, gameState, writer);

        var activeAfterEndTurn2Event = gameState.TurnAutomat.ActivePlayer;

        writer.Complete();

        // Assert
        activeAfterDefinedEvent.Should().Be(p1);
        activeAfterEndTurn1Event.Should().Be(p2);
        activeAfterEndTurn2Event.Should().Be(p1);

        reader.Count.Should().Be(3);

        (await reader.ReadAllAsync(TestContext.Current.CancellationToken)
            .ToListAsync(TestContext.Current.CancellationToken))
            .Should().SatisfyRespectively(
                first => first.Should().BeOfType<TurnStartedEvent>()
                        .Which.PlayerId.Should().Be(p1),
                second => second.Should().BeOfType<TurnStartedEvent>()
                        .Which.PlayerId.Should().Be(p2),
                last => last.Should().BeOfType<TurnStartedEvent>()
                        .Which.PlayerId.Should().Be(p1));
            
    }
}