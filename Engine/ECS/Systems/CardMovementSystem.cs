using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.ECS.Systems;

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

        var zone = gameState.Entities.Get<Zone>(@event.NewZoneId);
        var newOwnerId = zone.Get<OwnerComponent>().CurrentOwnerId;

        var card = gameState.Entities.Get<Card>(@event.CardId);
        card.Get<OwnerComponent>().ChangeOwnerId(newOwnerId);
        card.Get<ZoneComponent>().ChangeZoneId(zone.Id);
    }
}
