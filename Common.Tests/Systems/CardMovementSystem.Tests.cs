using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class CardMovementSystemTests
{
    [Fact]
    public void OnEventShouldMoveCardToCorrectHandZoneForSpecificPlayer()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new PlayerId(Guid.NewGuid());
        var otherPlayerId = new PlayerId(Guid.NewGuid());
        var cardId = new CardId(1);
        var targetZoneId = new ZoneId(10);

        var card = new Card(cardId);
        card.Add(new OwnerComponent(PlayerId.System));
        card.Add(new ZoneComponent(new ZoneId(0)));

        var handZone = new Zone(targetZoneId);
        handZone.Add(new OwnerComponent(playerId));
        handZone.Add(new ZoneRoleComponent(ZoneRole.Hand));

        var otherZone = new Zone(new ZoneId(99));
        otherZone.Add(new OwnerComponent(otherPlayerId));
        otherZone.Add(new ZoneRoleComponent(ZoneRole.Hand));

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.Setup(x => x.Get<Card, CardId>(cardId))
            .Returns(card);
        stateMock.Setup(x => x.Query<Zone>())
            .Returns([otherZone, handZone]);

        var channel = Channel.CreateUnbounded<IGameEvent>();

        var system = new CardMovementSystem(Mock.Of<IGameEngineLogger>());

        // Act
        var @event = new CardDealtEvent(gameId, cardId, playerId);
        system.OnEvent(@event, stateMock.Object, channel.Writer);

        // Assert
        card.Get<OwnerComponent>().CurrentOwnerId.Should().Be(playerId);
        card.Get<ZoneComponent>().CurrentZoneId.Should().Be(targetZoneId);
    }

    [Fact]
    public void OnEventShouldThrowWhenStateReturnsNoMatchingHandZone()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new PlayerId(Guid.NewGuid());
        var cardId = new CardId(1);

        var card = new Card(cardId);
        card.Add(new OwnerComponent(PlayerId.System));
        card.Add(new ZoneComponent(new ZoneId(0)));

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.Setup(x => x.Get<Card, CardId>(cardId))
            .Returns(card);
        stateMock.Setup(x => x.Query<Zone>())
            .Returns([]);

        var system = new CardMovementSystem(Mock.Of<IGameEngineLogger>());

        // Act
        var act = () => system.OnEvent(
            new CardDealtEvent(gameId, cardId, playerId),
            stateMock.Object,
            Channel.CreateUnbounded<IGameEvent>().Writer);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}