using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.Events.Domain;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events.Domain;

public sealed class TurnEndedEventTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new EntityId(3);

        // Act
        var @event = new TurnEndedEvent(gameId, playerId);

        // Assert
        @event.GameId.Should().Be(gameId);
        @event.PlayerId.Should().Be(playerId);
    }
}

