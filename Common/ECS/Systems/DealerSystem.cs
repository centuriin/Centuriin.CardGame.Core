using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Components.Zones;
using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Common.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.ECS.Systems;

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

    public void OnEvent(GameStartedEvent @event)
    {
        ValidateAndLog(@event);

        var playersDecks = GameState
            .Query<Card>()
            .ToLookup(k => k.Get<OwnerComponent>().CurrentOwnerId, v => v.Id)
            .ToDictionary(k => k.Key, v => new Queue<EntityId>(v.Shuffle()));

        foreach (var zone in GameState.Query<Zone>().WithComponent<HasPrimaryCards>())
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

                EventBusWriter.Write(childEvent);
            }
        }
    }
}
