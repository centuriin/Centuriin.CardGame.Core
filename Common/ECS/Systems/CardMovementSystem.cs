using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Common.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.ECS.Systems;

public sealed class CardMovementSystem :
    ISystem,
    IConsumingSubscriber<CardDealtEvent>
{
    public CardMovementSystem()
    {
    }

    public void OnEvent(CardDealtEvent @event, IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(gameState);

        var zone = gameState.Get<Zone>(@event.NewZoneId);
        var newOwnerId = zone.Get<OwnerComponent>().CurrentOwnerId;

        var card = gameState.Get<Card>(@event.CardId);
        card.Get<OwnerComponent>().ChangeOwnerId(newOwnerId);
        card.Get<ZoneComponent>().ChangeZoneId(zone.Id);
    }
}
