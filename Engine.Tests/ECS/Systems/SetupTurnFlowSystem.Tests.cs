using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Systems;

public sealed class SetupTurnFlowSystemTests
{
    [Fact(DisplayName = "Can define a cyclic turn order containing every participant exactly once and excluding the bank.")]
    [Trait("Category", "Unit")]
    public void CanDefineParticipantTurnOrder()
    {
        // Arrange
        Player[] players = [
            new(EntityId.Default, [new PlayerRoleComponent(PlayerRole.Bank)]),
            new(new(1), [new PlayerRoleComponent(PlayerRole.Participant)]),
            new(new(2), [new PlayerRoleComponent(PlayerRole.Participant)]),
            new(new(3))
        ];

        var gameId = new GameId(Guid.NewGuid());

        var events = new List<TurnFlowDefinedEvent>();

        var entities = new Mock<IReadOnlyEntitiesCollection>(MockBehavior.Strict);
        entities.Setup(x => x.Query<Player>())
            .Returns(players);

        var state = new Mock<IReadOnlyGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(entities.Object);

        var writer = new Mock<IGameEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<TurnFlowDefinedEvent>()))
            .Callback<IGameEvent>(e => events.Add((TurnFlowDefinedEvent)e));

        var system = new SetupTurnFlowSystem();

        // Act
        system.OnEvent(new GameStartedEvent(gameId), state.Object, writer.Object);

        // Assert
        events.Should().ContainSingle();
        events[0].GameId.Should().Be(gameId);
        events[0].IsCycled.Should().BeTrue();
        events[0].InitialPlayerTurnsOrder.Should().BeEquivalentTo(new EntityId[] { new(1), new(2) });
    }

    [Fact(DisplayName = "Cannot define turn order without participants.")]
    [Trait("Category", "Unit")]
    public void CanNotDefineEmptyTurnOrder()
    {
        // Arrange
        Player[] players = [new(EntityId.Default, [new PlayerRoleComponent(PlayerRole.Bank)])];

        var entities = new Mock<IReadOnlyEntitiesCollection>(MockBehavior.Strict);
        entities.Setup(x => x.Query<Player>())
            .Returns(players);

        var state = new Mock<IReadOnlyGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(entities.Object);

        var system = new SetupTurnFlowSystem();

        // Act
        var exception = Record.Exception(() =>
            system.OnEvent(
                new GameStartedEvent(new(Guid.NewGuid())),
                state.Object,
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = "Cannot configure turn order without an event.")]
    [Trait("Category", "Unit")]
    public void CanNotHandleWithoutEvent()
    {
        // Act
        var exception = Record.Exception(() =>
            new SetupTurnFlowSystem().OnEvent(
                null!,
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict),
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot configure turn order without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotHandleWithoutState()
    {
        // Act
        var exception = Record.Exception(() =>
            new SetupTurnFlowSystem().OnEvent(
                new GameStartedEvent(new(Guid.NewGuid())),
                null!,
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot configure turn order without an event writer.")]
    [Trait("Category", "Unit")]
    public void CanNotHandleWithoutWriter()
    {
        // Act
        var exception = Record.Exception(() =>
            new SetupTurnFlowSystem().OnEvent(
                new GameStartedEvent(new(Guid.NewGuid())),
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
