using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class TurnFlowSystem :
    SystemBase,
    ISubscriber<TurnFlowDefinedEvent>,
    ISubscriber<TurnEndedEvent>
{
    public TurnFlowSystem(
        IGameState gameState,
        IGameEventBusWriter eventBusWriter,
        ICoreLogger<TurnFlowSystem> logger) :
        base(gameState, eventBusWriter, logger)
    {
    }

    public void OnEvent(TurnFlowDefinedEvent @event)
    {
        ValidateAndLog(@event);

        if (@event.IsCycled)
        {
            GameState.TurnAutomat.SetCycle(@event.InitialPlayerTrunsOrder);
        }
        else
        {
            GameState.TurnAutomat.SetNext(@event.InitialPlayerTrunsOrder);
        }

        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug(
                "Turn flow defined in order {PlayersOrder} and cycled {IsCycled}",
                string.Join(' ', @event.InitialPlayerTrunsOrder.Select(x => x.Value)),
                @event.IsCycled);
        }

        EventBusWriter.Write(new TurnStartedEvent(@event.GameId, GameState.TurnAutomat.ActivePlayer));
    }

    public void OnEvent(TurnEndedEvent @event)
    {
        ValidateAndLog(@event);

        GameState.TurnAutomat.MoveNext();

        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug(
                "End turn for {PreviosPlayerId} and active player {CurrentPlayerId}",
                @event.PlayerId.Value,
                GameState.TurnAutomat.ActivePlayer.Value);
        }

        EventBusWriter.Write(new TurnStartedEvent(@event.GameId, GameState.TurnAutomat.ActivePlayer));
    }
}