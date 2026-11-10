using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.Events.Domain;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Events.Domain;

public sealed class TurnFlowDefinedEventTests
{
    [Theory(DisplayName = "Can be created with the specified turn order and cycle mode.")]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "Unit")]
    public void CanBeCreated(bool isCycled)
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        EntityId[] players = [new(3), new(1), new(2)];

        // Act
        var @event = new TurnFlowDefinedEvent(gameId, players, isCycled);

        // Assert
        @event.GameId.Should().Be(gameId);
        @event.InitialPlayerTurnsOrder.Should().Equal(players);
        @event.IsCycled.Should().Be(isCycled);
    }

    [Fact(DisplayName = "Cannot be created without an initial player turn order.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutInitialPlayerTurnsOrder()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        // Act
        var exception = Record.Exception(() =>
            new TurnFlowDefinedEvent(gameId, null!, true));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created with an empty initial player turn order.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithEmptyInitialPlayerTurnsOrder()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        // Act
        var exception = Record.Exception(() =>
            new TurnFlowDefinedEvent(gameId, [], false));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = "Can be created with a single player.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithSinglePlayer()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        EntityId[] players = [new(1)];

        // Act
        var @event = new TurnFlowDefinedEvent(gameId, players, false);

        // Assert
        @event.InitialPlayerTurnsOrder.Should().Equal(players);
    }
}
