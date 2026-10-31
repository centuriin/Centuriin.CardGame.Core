using Centuriin.CardGame.Core.Common.ECS.Components.Players;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.ECS.Systems;

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

        var orderedPlayerIds = readOnlyGameState.Query<Player>()
            .WithComponent<PlayerRoleComponent>(x => x.Role == PlayerRole.Participant)
            .Select(x => x.Id)
            .Shuffle()
            .ToList();

        eventBusWriter.Write(
            new TurnFlowDefinedEvent(
                @event.GameId,
                orderedPlayerIds,
                IsCycled: true));
    }
}
