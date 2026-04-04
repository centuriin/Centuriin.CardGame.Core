using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.SmokeTests;

public sealed class GameTests
{
    [Fact]
    [Trait("Category", "Smoke")]
    public async Task FullGameStartFlowShouldWorkCorrectlyAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new PlayerId(Guid.NewGuid());

        var handZone = new Zone(new ZoneId(10));
        handZone.Add(
            [
                new OwnerComponent(playerId),
                new ZoneRoleComponent(ZoneRole.Hand),
                new HasPrimaryCards(1)
            ]);

        var cardId = new CardId(1);
        var card = new Card(cardId);
        card.Add(
            [
                new OwnerComponent(PlayerId.System),
                new ZoneComponent(new ZoneId(0))
            ]);

        var gameState = new GameState(gameId, Mock.Of<ITurnAutomat>(MockBehavior.Strict));
        gameState.AddEntity<Player, PlayerId>(new Player(playerId));
        gameState.AddEntity<Player, PlayerId>(new Player(PlayerId.System));
        gameState.AddEntity<Zone, ZoneId>(handZone);
        gameState.AddEntity<Card, CardId>(card);

        var dealerSystem = new ClassicDealerSystem(DebugLogger.Instance);
        var movementSystem = new CardMovementSystem(DebugLogger.Instance);

        var dispatcher = new EventDispatcher();
        dispatcher.Register<CardDealtEvent>(movementSystem);
        dispatcher.Register<GameStartedEvent>(dealerSystem);

        var eventsRepo = new FakeEventsRepository();

        var game = new Game(gameState, eventsRepo, dispatcher);

        // Act
        await game.ApplyAsync(new GameStartedEvent(gameId), TestContext.Current.CancellationToken);

        // Assert
        var updatedCard = gameState.Get<Card, CardId>(cardId);
        updatedCard.Get<OwnerComponent>().CurrentOwnerId.Should().Be(playerId);
        updatedCard.Get<ZoneComponent>().CurrentZoneId.Should().Be(handZone.Id);

        eventsRepo.Events.Should().HaveCount(2); // GameStarted + CardDealt
        eventsRepo.Events[0].Should().BeOfType<GameStartedEvent>();
        eventsRepo.Events[1].Should().BeOfType<CardDealtEvent>();
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