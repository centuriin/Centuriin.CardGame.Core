using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.Events.Domain;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events.Domain;

public sealed class CardDealtEventTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var cardId = new EntityId(1);
        var zoneId = new EntityId(2);

        // Act
        var @event = new CardDealtEvent(gameId, cardId, zoneId);

        // Assert
        @event.GameId.Should().Be(gameId);
        @event.CardId.Should().Be(cardId);
        @event.NewZoneId.Should().Be(zoneId);
    }
}

