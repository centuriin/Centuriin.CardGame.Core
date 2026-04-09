using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class SetupTurnFlowSystem :
    SystemBase,
    ISubscriber<GameStartedEvent>
{
    public SetupTurnFlowSystem(IGameEngineLogger logger) : base(logger)
    {
    }

    public void OnEvent(GameStartedEvent @event, IGameState gameState, IEventBusWriter writer)
    {
        ValidateAndLog(@event, gameState, writer);

        var orderedPlayerIds = gameState.Query<Player>()
            .WithComponent<PlayerRoleComponent>(x => x.Role == PlayerRole.Participant)
            .As<Player>()
            .Select(x => x.Id)
            .Shuffle()
            .ToList();

        writer.Write(new TurnFlowDefinedEvent(@event.GameId, orderedPlayerIds, IsCycled: true));
    }
}
