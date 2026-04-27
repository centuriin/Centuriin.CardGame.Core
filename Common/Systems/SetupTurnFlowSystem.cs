using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Systems;

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
