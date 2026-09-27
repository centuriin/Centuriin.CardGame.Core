using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Systems;

public sealed class SetupTurnFlowSystemTests
{
    [Fact]
    public void OnEventShouldDefineTurnFlowOnlyForParticipants()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var p1Id = new EntityId(1);
        var p2Id = new EntityId(2);
        var systemId = EntityId.Default;

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
        var writer = new Mock<IGameEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<IGameEvent>()))
            .Callback((IGameEvent e) => eventsList.Add(e));

        var system = new SetupTurnFlowSystem(
            stateMock.Object, 
            writer.Object, 
            Mock.Of<ICoreLogger<SetupTurnFlowSystem>>());

        // Act
        var @event = new GameStartedEvent(gameId);
        system.OnEvent(@event);

        // Assert
        eventsList.Single().Should()
            .Match<TurnFlowDefinedEvent>(e =>
                e.GameId == gameId &&
                e.InitialPlayerTurnsOrder.Count == 2 &&
                e.InitialPlayerTurnsOrder.Contains(p1Id) &&
                e.InitialPlayerTurnsOrder.Contains(p2Id) &&
                !e.InitialPlayerTurnsOrder.Contains(systemId) &&
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
        var writer = new Mock<IGameEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<IGameEvent>()))
            .Callback((IGameEvent e) => eventsList.Add(e));

        var system = new SetupTurnFlowSystem(
            stateMock.Object, 
            writer.Object, 
            Mock.Of<ICoreLogger<SetupTurnFlowSystem>>());

        // Act
        system.OnEvent(new GameStartedEvent(gameId));

        // Assert
        eventsList.Single().Should()
            .Match<TurnFlowDefinedEvent>(e =>
                e.GameId == gameId &&
                e.InitialPlayerTurnsOrder.Count == 0 &&
                e.IsCycled == true);
    }
}