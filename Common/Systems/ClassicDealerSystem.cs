using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class ClassicDealerSystem : 
    SubscriberBase, 
    ISystem,
    ISubscriber<GameStartedEvent>
{
    public ClassicDealerSystem(IGameEngineLogger logger) : base(logger)
    {
    }

    public void OnEvent(GameStartedEvent @event, IGameState gameState, ChannelWriter<IGameEvent> writer)
    {
        ValidateAndLog(@event, gameState, writer);

        var generalDeck = gameState
            .Query<Card>()
            .WithComponent<OwnerComponent>(x => x.CurrentOwnerId == PlayerId.System)
            .As<Card>()
            .Select(x => x.Id)
            .Shuffle();

        var pickQueue = new Queue<CardId>(generalDeck);

        foreach (var zone in gameState.Query<Zone>().WithComponent<HasPrimaryCards>())
        {
            var cardCount = zone.Get<HasPrimaryCards>().Count;

            for (var i = 0; i < cardCount; i++)
            {
                var pickedCardId = pickQueue.Dequeue();

                var childEvent = new CardDealtEvent(
                    gameState.GameId,
                    pickedCardId,
                    zone.Get<OwnerComponent>().CurrentOwnerId);

                _ = writer.TryWrite(childEvent);
            }
        }
    }
}
