using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class DealerSystem : 
    SystemBase,
    ISubscriber<GameStartedEvent>
{
    public DealerSystem(IGameEngineLogger logger) : base(logger)
    {
    }

    public void OnEvent(GameStartedEvent @event, IGameState gameState, ChannelWriter<IGameEvent> writer)
    {
        ValidateAndLog(@event, gameState, writer);

        var playersDecks = gameState
            .Query<Card>()
            .ToLookup(k => k.Get<OwnerComponent>().CurrentOwnerId, v => v.Id)
            .ToDictionary(k => k.Key, v => new Queue<CardId>(v.Shuffle()));

        foreach (var zone in gameState.Query<Zone>().WithComponent<HasPrimaryCards>())
        {
            var cardCount = zone.Get<HasPrimaryCards>().Count;
            var zoneOwner = zone.Get<OwnerComponent>().CurrentOwnerId;

            var deckQueue = playersDecks.ContainsKey(zoneOwner)
                ? playersDecks[zoneOwner]
                : playersDecks[PlayerId.System];

            for (var i = 0; i < cardCount; i++)
            {
                var pickedCardId = deckQueue.Dequeue();

                var childEvent = new CardDealtEvent(
                    @event.GameId,
                    pickedCardId,
                    zone.Get<OwnerComponent>().CurrentOwnerId);

                _ = writer.TryWrite(childEvent);
            }
        }
    }
}
