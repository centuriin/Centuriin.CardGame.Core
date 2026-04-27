using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Systems;

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

    public void OnEvent(
        CardDealtEvent @event,
        IGameState gameState,
        IGameEventBus writer)
    {
        ValidateAndLog(@event, gameState, writer);

        var card = gameState.Get<Card>(@event.CardId);
        card.Get<OwnerComponent>().ChangeOwnerId(@event.NewOwnerId);

        var hand = gameState
            .Query<Zone>()
            .WithComponent<OwnerComponent>(x => x.CurrentOwnerId == @event.NewOwnerId)
            .WithComponent<ZoneRoleComponent>(x => x.Role == ZoneRole.Hand)
            .Single();

        card.Get<ZoneComponent>().ChangeZoneId(hand.Id);
    }
}
