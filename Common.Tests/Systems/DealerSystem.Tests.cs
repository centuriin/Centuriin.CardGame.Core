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

public sealed class DealerSystemTests
{
    [Fact]
    public void OnEventCoreShouldGenerateEventsForAllZonesWithHasPrimaryCards()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId1 = new PlayerId(Guid.NewGuid());
        var playerId2 = new PlayerId(Guid.NewGuid());

        var cardId1 = new CardId(1);
        var card1 = new Card(cardId1);
        card1.Add(new OwnerComponent(PlayerId.System));

        var cardId2 = new CardId(2);
        var card2 = new Card(new(2));
        card2.Add(new OwnerComponent(PlayerId.System));

        var zoneId = new ZoneId(10);
        var zone1 = new Zone(zoneId);
        zone1.Add(new OwnerComponent(playerId1), new HasPrimaryCards(1));

        var zoneId2 = new ZoneId(20);
        var zone2 = new Zone(zoneId2);
        zone2.Add(new OwnerComponent(playerId2), new HasPrimaryCards(1));

        var stateMock = new Mock<IGameState>(MockBehavior.Strict);
        stateMock.Setup(x => x.Query<Card>())
            .Returns([card1, card2]);
        stateMock.Setup(x => x.Query<Zone>())
            .Returns([zone1, zone2]);

        var eventsList = new List<IGameEvent>();
        var writer = new Mock<IEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<IGameEvent>()))
            .Callback((IGameEvent e) => eventsList.Add(e));

        var system = new DealerSystem(Mock.Of<IGameEngineLogger>());

        // Act
        system.OnEvent(new GameStartedEvent(gameId), stateMock.Object, writer.Object);

        // Assert
        eventsList.Count.Should().Be(2);

        var dealtEvent = eventsList[0].Should().BeOfType<CardDealtEvent>().Which;
        var dealtEvent2 = eventsList[1].Should().BeOfType<CardDealtEvent>().Which;

        dealtEvent.CardId.Should().NotBe(dealtEvent2.CardId);
        dealtEvent.NewOwnerId.Should().NotBe(dealtEvent2.NewOwnerId);
    }
}