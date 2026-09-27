using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Common.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.ECS.Systems;

public sealed class CardMovementSystem :
    SystemBase,
    ISubscriber<CardDealtEvent>
{
    public CardMovementSystem(
        IGameState gameState,
        IGameEventBusWriter eventBusWriter,
        ICoreLogger<CardMovementSystem> logger) :
        base(gameState, eventBusWriter, logger)
    {
    }

    public void OnEvent(CardDealtEvent @event)
    {
        ValidateAndLog(@event);

        var zone = GameState.Get<Zone>(@event.NewZoneId);
        var newOwnerId = zone.Get<OwnerComponent>().CurrentOwnerId;

        var card = GameState.Get<Card>(@event.CardId);
        card.Get<OwnerComponent>().ChangeOwnerId(newOwnerId);
        card.Get<ZoneComponent>().ChangeZoneId(zone.Id);
    }
}
