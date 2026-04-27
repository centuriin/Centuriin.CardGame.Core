using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class DealerSystem :
    SystemBase,
    ISubscriber<GameStartedEvent>
{
    public DealerSystem(
        IGameState gameState,
        IGameEventBusWriter eventBusWriter,
        ICoreLogger<DealerSystem> logger) : 
        base(gameState, eventBusWriter, logger)
    {
    }

    public void OnEvent(GameStartedEvent @event, IGameState gameState, IGameEventBus writer)
    {
        ValidateAndLog(@event, gameState, writer);

        var playersDecks = gameState
            .Query<Card>()
            .ToLookup(k => k.Get<OwnerComponent>().CurrentOwnerId, v => v.Id)
            .ToDictionary(k => k.Key, v => new Queue<EntityId>(v.Shuffle()));

        foreach (var zone in gameState.Query<Zone>().WithComponent<HasPrimaryCards>())
        {
            var cardCount = zone.Get<HasPrimaryCards>().Count;
            var zoneOwner = zone.Get<OwnerComponent>().CurrentOwnerId;

            var deckQueue = playersDecks.ContainsKey(zoneOwner)
                ? playersDecks[zoneOwner]
                : playersDecks[EntityId.Default];

            for (var i = 0; i < cardCount; i++)
            {
                var pickedCardId = deckQueue.Dequeue();

                var childEvent = new CardDealtEvent(
                    @event.GameId,
                    pickedCardId,
                    zone.Get<OwnerComponent>().CurrentOwnerId);

                writer.Write(childEvent);
            }
        }
    }
}
