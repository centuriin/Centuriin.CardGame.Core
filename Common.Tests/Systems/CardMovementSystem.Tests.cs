using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class CardMovementSystemTests
{
    [Fact]
    public void OnEventShouldUpdateCardOwnerAndZoneToPlayerHand()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new PlayerId(Guid.NewGuid());
        var cardId = new CardId(1);
        var handZoneId = new ZoneId(10);

        var card = new Card(cardId);
        card.Add(new OwnerComponent(PlayerId.System));
        card.Add(new ZoneComponent(new ZoneId(0)));

        var handZone = new Zone(handZoneId);
        handZone.Add(new OwnerComponent(playerId));
        handZone.Add(new ZoneRoleComponent(ZoneRole.Hand));

        var gameStateMock = new Mock<IGameState>(MockBehavior.Strict);
        gameStateMock
            .Setup(x => x.Get<Card, CardId>(cardId))
            .Returns(card);
        gameStateMock
            .Setup(x => x.Query<Zone>())
            .Returns([handZone]);

        var system = new CardMovementSystem();
        var @event = new CardDealtEvent(gameId, cardId, playerId);
        var writer = Channel.CreateUnbounded<IGameEvent>().Writer;

        // Act
        var exception = Record.Exception(() =>
            system.OnEvent(@event, gameStateMock.Object, writer));

        // Assert
        exception.Should().BeNull();
        card.Get<OwnerComponent>().CurrentOwnerId.Should().Be(playerId);
        card.Get<ZoneComponent>().CurrentZoneId.Should().Be(handZoneId);
    }

    [Fact]
    public void OnEventShouldThrowInvalidOperationExceptionWhenHandZoneIsMissing()
    {
        // Arrange
        var playerId = new PlayerId(Guid.NewGuid());
        var cardId = new CardId(1);

        var card = new Card(cardId);
        card.Add(new OwnerComponent(PlayerId.System));

        var gameStateMock = new Mock<IGameState>(MockBehavior.Strict);
        gameStateMock
            .Setup(x => x.Get<Card, CardId>(cardId))
            .Returns(card);
        gameStateMock
            .Setup(x => x.Query<Zone>())
            .Returns([]);

        var system = new CardMovementSystem();
        var @event = new CardDealtEvent(new GameId(Guid.NewGuid()), cardId, playerId);
        var writer = Channel.CreateUnbounded<IGameEvent>().Writer;

        // Act
        var exception = Record.Exception(() =>
            system.OnEvent(@event, gameStateMock.Object, writer));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<InvalidOperationException>();
    }
}