using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class SetupTurnFlowSystemTests
{
    [Fact]
    public void OnEventShouldDefineTurnFlowOnlyForParticipants()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var p1Id = new PlayerId(Guid.NewGuid());
        var p2Id = new PlayerId(Guid.NewGuid());
        var systemId = PlayerId.System;

        var player1 = new Player(p1Id);
        player1.Add(new PlayerRoleComponent(PlayerRole.Participant));

        var player2 = new Player(p2Id);
        player2.Add(new PlayerRoleComponent(PlayerRole.Participant));

        var systemPlayer = new Player(systemId);
        systemPlayer.Add(new PlayerRoleComponent(PlayerRole.Bank));

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.Setup(x => x.Query<Player>())
            .Returns([player1, systemPlayer, player2]);

        var eventsList = new List<IGameEvent>();
        var writer = new Mock<IEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<IGameEvent>()))
            .Callback((IGameEvent e) => eventsList.Add(e));

        var system = new SetupTurnFlowSystem(Mock.Of<IGameEngineLogger>());

        // Act
        var @event = new GameStartedEvent(gameId);
        system.OnEvent(@event, stateMock.Object, writer.Object);

        // Assert
        eventsList.Single().Should()
            .Match<TurnFlowDefinedEvent>(e =>
                e.GameId == gameId &&
                e.InitialPlayerTrunsOrder.Count == 2 &&
                e.InitialPlayerTrunsOrder.Contains(p1Id) &&
                e.InitialPlayerTrunsOrder.Contains(p2Id) &&
                !e.InitialPlayerTrunsOrder.Contains(systemId) &&
                e.IsCycled == true);
    }

    [Fact]
    public void OnEventShouldProduceEventWithEmptyOrderWhenNoParticipantsFound()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.Setup(x => x.Query<Player>())
            .Returns([]);

        var eventsList = new List<IGameEvent>();
        var writer = new Mock<IEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<IGameEvent>()))
            .Callback((IGameEvent e) => eventsList.Add(e));

        var system = new SetupTurnFlowSystem(Mock.Of<IGameEngineLogger>());

        // Act
        system.OnEvent(new GameStartedEvent(gameId), stateMock.Object, writer.Object);

        // Assert
        eventsList.Single().Should()
            .Match<TurnFlowDefinedEvent>(e =>
                e.GameId == gameId &&
                e.InitialPlayerTrunsOrder.Count == 0 &&
                e.IsCycled == true);
    }
}