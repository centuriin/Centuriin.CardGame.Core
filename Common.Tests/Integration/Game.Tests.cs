using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Integration;

public sealed class GameTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task FullGameStartFlowShouldWorkCorrectlyAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var playerId = new EntityId(1);

        var handZone = new Zone(new(10));
        handZone.Add(
            [
                new OwnerComponent(playerId),
                new ZoneRoleComponent(ZoneRole.Hand),
                new HasPrimaryCards(1)
            ]);

        var cardId = new EntityId(1);
        var card = new Card(cardId);
        card.Add(
            [
                new OwnerComponent(EntityId.Default),
                new ZoneComponent(new(0))
            ]);

        var gameState = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));
        gameState.AddEntity(new Player(playerId));
        gameState.AddEntity(new Player(EntityId.Default));
        gameState.AddEntity(handZone);
        gameState.AddEntity(card);

        var dealerSystem = new DealerSystem(DebugLogger<DealerSystem>.Instance);
        var movementSystem = new CardMovementSystem(DebugLogger<CardMovementSystem>.Instance);

        var dispatcher = new EventDispatcher();
        dispatcher.Register<CardDealtEvent>(movementSystem);
        dispatcher.Register<GameStartedEvent>(dealerSystem);

        var eventsRepo = new FakeEventsRepository();

        var game = new Game(
            gameId,
            gameState,
            Mock.Of<ICommandValidator>(MockBehavior.Strict),
            eventsRepo,
            dispatcher);

        // Act
        await game.ApplyAsync(new GameStartedEvent(gameId), TestContext.Current.CancellationToken);

        // Assert
        var updatedCard = gameState.Get<Card>(cardId);
        updatedCard.Get<OwnerComponent>().CurrentOwnerId.Should().Be(playerId);
        updatedCard.Get<ZoneComponent>().CurrentZoneId.Should().Be(handZone.Id);

        eventsRepo.Events.Should().HaveCount(1);

        var eventUnit = eventsRepo.Events.Single();

        eventUnit.PrimaryEvent.Should().BeOfType<GameStartedEvent>();
        eventUnit.RelatedRandomEvents.Single().Should().BeOfType<CardDealtEvent>();
    }

    private class FakeEventsRepository : IGameEventsRepository
    {
        public List<IGameEventUnit> Events { get; } = new();
        public Task AddAsync(IGameEventUnit @event, CancellationToken token)
        {
            Events.Add(@event);
            return Task.CompletedTask;
        }
    }
}