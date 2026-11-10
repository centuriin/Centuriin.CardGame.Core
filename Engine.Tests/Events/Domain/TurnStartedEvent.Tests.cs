using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.Events.Domain;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Events.Domain;

public sealed class TurnStartedEventTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new EntityId(3);

        // Act
        var @event = new TurnStartedEvent(gameId, playerId);

        // Assert
        @event.GameId.Should().Be(gameId);
        @event.PlayerId.Should().Be(playerId);
    }
}

