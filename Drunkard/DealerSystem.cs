using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Components.Players;
using Centuriin.CardGame.Core.Common.ECS.Components.Zones;
using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Drunkard;

internal sealed class DealerSystem : SystemBase, IDealerSystem
{
    private const int PLAYERS_COUNT = 2;

    public DealerSystem(
        IGameState gameState,
        IGameEventBusWriter eventBusWriter,
        ICoreLogger<DealerSystem> logger) :
        base(gameState, eventBusWriter, logger)
    {
    }

    public void OnEvent(GameStartedEvent @event)
    {
        ValidateAndLog(@event);

        var deckId = GameState.Query<Zone>()
            .WithComponent<ZoneRoleComponent>(x => x.Role is ZoneRole.Deck)
            .Single()
            .Id;

        var deckCards = GameState.Query<Card>()
            .WithComponent<ZoneComponent>(x => x.CurrentZoneId == deckId)
            .Shuffle()
            .Cast<Card>()
            .ToList();

        var activePlayers = GameState.Query<Player>()
            .WithComponent<PlayerRoleComponent>(x =>
                x.Role.HasFlag(PlayerRole.Participant))
            .ToList();

        if (activePlayers.Count != PLAYERS_COUNT)
        {
            throw new InvalidOperationException(
                $"Active players count should be {PLAYERS_COUNT}," +
                $" but found {activePlayers.Count}");
        }

        var cardPerPlayerCount = deckCards.Count / PLAYERS_COUNT;

        DealCardsToPlayer(
            @event.GameId,
            activePlayers.First().Id,
            deckCards.Take(cardPerPlayerCount));

        DealCardsToPlayer(
            @event.GameId,
            activePlayers.Last().Id,
            deckCards.TakeLast(cardPerPlayerCount));
    }

    // Skip TurnEndedEvent
    public void OnEvent(TurnEndedEvent @event) { }

    private void DealCardsToPlayer(
        GameId gameId,
        EntityId playerId, 
        IEnumerable<Card> cards)
    {
        var hand = GameState.Query<Zone>()
            .WithComponent<OwnerComponent>(x => x.CurrentOwnerId == playerId)
            .Single();

        foreach (var card in cards)
        {
            var @event = new CardDealtEvent(gameId, card.Id, hand.Id);
            EventBusWriter.Write(@event);
        }
    }
}
