using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameSmokeTests
{
    [Fact]
    [Trait("Category", "Smoke")]
    public async Task FullGameStartFlowShouldWorkCorrectlyAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new PlayerId(Guid.NewGuid());

        var handZone = new Zone(new ZoneId(10));
        handZone.Add(new OwnerComponent(playerId));
        handZone.Add(new ZoneRoleComponent(ZoneRole.Hand));

        var cardId = new CardId(1);
        var card = new Card(cardId);
        card.Add(new OwnerComponent(PlayerId.System));
        card.Add(new ZoneComponent(new ZoneId(0)));

        var gameState = new GameState(gameId);
        gameState.AddEntity<Player, PlayerId>(new Player(playerId));
        gameState.AddEntity<Player, PlayerId>(new Player(PlayerId.System));
        gameState.AddEntity<Zone, ZoneId>(handZone);
        gameState.AddEntity<Card, CardId>(card);

        var movementSystem = new CardMovementSystem();
        var dispatcher = new EventDispatcher();
        dispatcher.Register<CardDealtEvent>(movementSystem);

        var eventsRepo = new FakeEventsRepository();

        var game = new Game(gameState, eventsRepo, dispatcher);

        var dealer = new ClassicDealerService(game);

        // Act
        await dealer.DealCardsAsync(count: 1, CancellationToken.None);

        // Assert
        var updatedCard = gameState.Get<Card, CardId>(cardId);
        updatedCard.Get<OwnerComponent>().CurrentOwnerId.Should().Be(playerId);
        updatedCard.Get<ZoneComponent>().CurrentZoneId.Should().Be(handZone.Id);

        eventsRepo.Events.Should().HaveCount(2); // CardDealt + GameStarted
        eventsRepo.Events[0].Should().BeOfType<CardDealtEvent>();
        eventsRepo.Events[1].Should().BeOfType<GameStartedEvent>();
    }

    private class FakeEventsRepository : IGameEventsRepository
    {
        public List<IGameEvent> Events { get; } = new();
        public Task AddAsync(IGameEvent @event, CancellationToken token)
        {
            Events.Add(@event);
            return Task.CompletedTask;
        }
    }
}