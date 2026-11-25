using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Systems;

public sealed class TurnFlowSystemTests
{
    [Theory(DisplayName = "Can apply a cyclic or finite turn order.")]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "Unit")]
    public void CanApplyTurnOrder(bool isCycled)
    {
        // Arrange
        EntityId[] players = [new(1), new(2)];

        IReadOnlyCollection<EntityId>? capturedPlayers = null;
        var cycleCalls = 0;
        var nextCalls = 0;
        var automat = new Mock<ITurnAutomat>(MockBehavior.Strict);
        automat.Setup(x => x.SetCycle(It.IsAny<IReadOnlyCollection<EntityId>>()))
            .Callback<IReadOnlyCollection<EntityId>>(ids =>
            {
                cycleCalls++;
                capturedPlayers = ids;
            });
        automat.Setup(x => x.SetNext(It.IsAny<IReadOnlyCollection<EntityId>>()))
            .Callback<IReadOnlyCollection<EntityId>>(ids =>
            {
                nextCalls++;
                capturedPlayers = ids;
            });

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.TurnAutomat)
            .Returns(automat.Object);

        var system = new TurnFlowSystem();

        // Act
        system.OnEvent(new TurnFlowDefinedEvent(
            new(Guid.NewGuid()),
            players,
            isCycled), state.Object);

        // Assert
        capturedPlayers.Should().Equal(players);
        cycleCalls.Should().Be(isCycled ? 1 : 0);
        nextCalls.Should().Be(isCycled ? 0 : 1);
    }

    [Fact(DisplayName = "Can advance to the next player when a turn ends.")]
    [Trait("Category", "Unit")]
    public void CanEndTurn()
    {
        // Arrange
        var moveCalls = 0;
        var automat = new Mock<ITurnAutomat>(MockBehavior.Strict);
        automat.Setup(x => x.MoveNext())
            .Callback(() => moveCalls++);

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.TurnAutomat)
            .Returns(automat.Object);

        var system = new TurnFlowSystem();

        // Act
        system.OnEvent(new TurnEndedEvent(new(Guid.NewGuid()), new(1)), state.Object);

        // Assert
        moveCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Can announce the active player after applying a turn order definition.")]
    [Trait("Category", "Unit")]
    public void CanAnnounceInitialTurn()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var events = new List<IGameEvent>();

        var automat = new Mock<IReadOnlyTurnAutomat>(MockBehavior.Strict);
        automat.SetupGet(x => x.ActivePlayer)
            .Returns(new EntityId(1));

        var state = new Mock<IReadOnlyGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.TurnAutomat)
            .Returns(automat.Object);

        var writer = new Mock<IGameEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<TurnStartedEvent>()))
            .Callback<IGameEvent>(events.Add);

        var system = new TurnFlowSystem();

        // Act
        system.OnEvent(
            new TurnFlowDefinedEvent(
                gameId,
                [new(1), new(2)],
                true),
            state.Object,
            writer.Object);

        // Assert
        events.Should().Equal(new TurnStartedEvent(gameId, new(1)));
    }

    [Fact(DisplayName = "Can announce the new active player after an ended turn was applied.")]
    [Trait("Category", "Unit")]
    public void CanAnnounceNextTurn()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var events = new List<IGameEvent>();

        var automat = new Mock<IReadOnlyTurnAutomat>(MockBehavior.Strict);
        automat.SetupGet(x => x.ActivePlayer)
            .Returns(new EntityId(2));

        var state = new Mock<IReadOnlyGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.TurnAutomat)
            .Returns(automat.Object);

        var writer = new Mock<IGameEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<TurnStartedEvent>()))
            .Callback<IGameEvent>(events.Add);

        var system = new TurnFlowSystem();

        // Act
        system.OnEvent(new TurnEndedEvent(gameId, new(1)), state.Object, writer.Object);

        // Assert
        events.Should().Equal(new TurnStartedEvent(gameId, new(2)));
    }

    [Fact(DisplayName = "Cannot consume definition without an event.")]
    [Trait("Category", "Unit")]
    public void CanNotConsumeDefinitionWithoutEvent()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent(
                (TurnFlowDefinedEvent)null!,
                Mock.Of<IGameState>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot consume definition without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotConsumeDefinitionWithoutState()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent(
                new TurnFlowDefinedEvent(
                    new(Guid.NewGuid()),
                    [new(1)],
                    true),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot produce from definition without an event.")]
    [Trait("Category", "Unit")]
    public void CanNotProduceDefinitionWithoutEvent()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent(
                (TurnFlowDefinedEvent)null!,
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict),
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot produce from definition without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotProduceDefinitionWithoutState()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent(
                new TurnFlowDefinedEvent(
                    new(Guid.NewGuid()),
                    [new(1)],
                    true),
                null!,
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot produce from definition without an event writer.")]
    [Trait("Category", "Unit")]
    public void CanNotProduceDefinitionWithoutWriter()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent(
                new TurnFlowDefinedEvent(
                    new(Guid.NewGuid()),
                    [new(1)],
                    true),
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot consume turn end without an event.")]
    [Trait("Category", "Unit")]
    public void CanNotConsumeTurnEndWithoutEvent()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent((TurnEndedEvent)null!, Mock.Of<IGameState>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot consume turn end without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotConsumeTurnEndWithoutState()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent(
                new TurnEndedEvent(new(Guid.NewGuid()), new(1)),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot produce from turn end without an event.")]
    [Trait("Category", "Unit")]
    public void CanNotProduceTurnEndWithoutEvent()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent(
                (TurnEndedEvent)null!,
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict),
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot produce from turn end without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotProduceTurnEndWithoutState()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent(
                new TurnEndedEvent(new(Guid.NewGuid()), new(1)),
                null!,
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot produce from turn end without an event writer.")]
    [Trait("Category", "Unit")]
    public void CanNotProduceTurnEndWithoutWriter()
    {
        // Act
        var exception = Record.Exception(() =>
            new TurnFlowSystem().OnEvent(
                new TurnEndedEvent(new(Guid.NewGuid()), new(1)),
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
