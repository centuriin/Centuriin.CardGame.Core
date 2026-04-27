using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Systems;

public sealed class CardMovementSystemTests
{
    [Fact]
    public void OnEventShouldMoveCardToCorrectHandZoneForSpecificPlayer()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new EntityId(1);
        var otherPlayerId = new EntityId(2);
        var cardId = new EntityId(1);
        var targetZoneId = new EntityId(10);

        var card = new Card(cardId);
        card.Add(
            [
                new OwnerComponent(EntityId.Default),
                new ZoneComponent(new(0))
            ]);

        var handZone = new Zone(targetZoneId);
        handZone.Add(
            [
                new OwnerComponent(playerId),
                new ZoneRoleComponent(ZoneRole.Hand)
            ]);

        var otherZone = new Zone(new(99));
        otherZone.Add(
            [
                new OwnerComponent(otherPlayerId),
                new ZoneRoleComponent(ZoneRole.Hand)
            ]);

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.Setup(x => x.Get<Card>(cardId))
            .Returns(card);
        stateMock.Setup(x => x.Query<Zone>())
            .Returns([otherZone, handZone]);

        var system = new CardMovementSystem(
            stateMock.Object,
            Mock.Of<IGameEventBusWriter>(),
            Mock.Of<ICoreLogger<CardMovementSystem>>());

        // Act
        var @event = new CardDealtEvent(gameId, cardId, playerId);
        system.OnEvent(@event);

        // Assert
        card.Get<OwnerComponent>().CurrentOwnerId.Should().Be(playerId);
        card.Get<ZoneComponent>().CurrentZoneId.Should().Be(targetZoneId);
    }

    [Fact]
    public void OnEventShouldThrowWhenStateReturnsNoMatchingHandZone()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new EntityId(1);
        var cardId = new EntityId(1);

        var card = new Card(cardId);
        card.Add(
            [
                new OwnerComponent(EntityId.Default),
                new ZoneComponent(new(0))
            ]);

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.Setup(x => x.Get<Card>(cardId))
            .Returns(card);
        stateMock.Setup(x => x.Query<Zone>())
            .Returns([]);

        var system = new CardMovementSystem(
            stateMock.Object,
            Mock.Of<IGameEventBusWriter>(),
            Mock.Of<ICoreLogger<CardMovementSystem>>());

        // Act
        var exception = Record.Exception(() =>
            system.OnEvent(new CardDealtEvent(gameId, cardId, playerId)));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }
}