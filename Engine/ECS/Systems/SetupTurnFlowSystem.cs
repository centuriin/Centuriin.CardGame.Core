using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.ECS.Systems;

public sealed class SetupTurnFlowSystem :
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

        var orderedPlayerIds = readOnlyGameState.Entities
            .Query<Player>()
            .WithComponent<PlayerRoleComponent>(x => x.Role == PlayerRole.Participant)
            .Select(x => x.Id)
            .Shuffle()
            .ToList();

        eventBusWriter.Write(
            new TurnFlowDefinedEvent(
                @event.GameId,
                orderedPlayerIds,
                isCycled: true));
    }
}
