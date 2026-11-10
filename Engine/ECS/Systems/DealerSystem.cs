using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.ECS.Systems;

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

        var playersDecks = readOnlyGameState.Entities
            .Query<Card>()
            .ToLookup(
                k => k.Get<OwnerComponent>().CurrentOwnerId,
                v => v.Id)
            .ToDictionary(
                k => k.Key,
                v => new Queue<EntityId>(v.Shuffle()));

        foreach (var zone in readOnlyGameState.Entities
            .Query<Zone>()
            .WithComponent<HasPrimaryCards>())
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
                    zone.Id);

                eventBusWriter.Write(childEvent);
            }
        }
    }
}
