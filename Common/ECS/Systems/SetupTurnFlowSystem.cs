using Centuriin.CardGame.Core.Common.ECS.Components.Players;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.ECS.Systems;

public sealed class SetupTurnFlowSystem :
    SystemBase,
    ISubscriber<GameStartedEvent>
{
    public SetupTurnFlowSystem(
        IGameState gameState,
        IGameEventBusWriter eventBusWriter,
        ICoreLogger<SetupTurnFlowSystem> logger) : 
        base(gameState, eventBusWriter, logger)
    {
    }

    public void OnEvent(GameStartedEvent @event)
    {
        ValidateAndLog(@event);

        var orderedPlayerIds = GameState.Query<Player>()
            .WithComponent<PlayerRoleComponent>(x => x.Role == PlayerRole.Participant)
            .Select(x => x.Id)
            .Shuffle()
            .ToList();

        EventBusWriter.Write(new TurnFlowDefinedEvent(@event.GameId, orderedPlayerIds, IsCycled: true));
    }
}
