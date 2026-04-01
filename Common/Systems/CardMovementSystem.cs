using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class CardMovementSystem :
    ISystem,
    ISubscriber<CardDealtEvent>
{
    public void OnEvent(
        CardDealtEvent @event,
        IGameState gameState,
        ChannelWriter<IGameEvent> writer)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(gameState);
        ArgumentNullException.ThrowIfNull(writer);

        var card = gameState.Get<Card, CardId>(@event.CardId);
        card.Get<OwnerComponent>().ChangeOwnerId(@event.NewOwnerId);

        var hand = gameState
            .Query<Zone>()
            .WithComponent<OwnerComponent>(x => x.CurrentOwnerId == @event.NewOwnerId)
            .WithComponent<ZoneRoleComponent>(x => x.Role == ZoneRole.Hand)
            .As<Zone>()
            .Single();

        card.Get<ZoneComponent>().ChangeZoneId(hand.Id);
    }
}
