using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Components.Zones;
using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Common.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.ECS.Systems;

public sealed class DealerSystem :
    ISystem,
    IProducingSubscriber<GameStartedEvent>
{
    public void OnEvent(
        GameStartedEvent @event,
        IReadOnlyGameState readOnlyGameState,
        IGameEventBusWriter eventBusWriter)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(readOnlyGameState);
        ArgumentNullException.ThrowIfNull(eventBusWriter);

        var playersDecks = readOnlyGameState
            .Query<Card>()
            .ToLookup(
                k => k.Get<OwnerComponent>().CurrentOwnerId,
                v => v.Id)
            .ToDictionary(
                k => k.Key,
                v => new Queue<EntityId>(v.Shuffle()));

        foreach (var zone in readOnlyGameState.Query<Zone>().WithComponent<HasPrimaryCards>())
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

                eventBusWriter.Write(childEvent);
            }
        }
    }
}
