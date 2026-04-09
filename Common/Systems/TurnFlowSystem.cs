using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;

namespace Centuriin.CardGame.Core.Common.Systems;

public sealed class TurnFlowSystem :
    SystemBase,
    ISubscriber<TurnFlowDefinedEvent>,
    ISubscriber<TurnEndedEvent>
{
    public TurnFlowSystem(IGameEngineLogger logger) : base(logger)
    {
    }

    public void OnEvent(TurnFlowDefinedEvent @event, IGameState gameState, IEventBusWriter writer)
    {
        ValidateAndLog(@event, gameState, writer);

        if (@event.IsCycled)
        {
            gameState.TurnAutomat.SetCycle(@event.InitialPlayerTrunsOrder);
        }
        else
        {
            gameState.TurnAutomat.SetNext(@event.InitialPlayerTrunsOrder);
        }

        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug(
                "Turn flow defined in order {PlayersOrder} and cycled {IsCycled}",
                string.Join(' ', @event.InitialPlayerTrunsOrder.Select(x => x.Value)),
                @event.IsCycled);
        }

        writer.Write(new TurnStartedEvent(@event.GameId, gameState.TurnAutomat.ActivePlayer));
    }

    public void OnEvent(TurnEndedEvent @event, IGameState gameState, IEventBusWriter writer)
    {
        ValidateAndLog(@event, gameState, writer);

        gameState.TurnAutomat.MoveNext();

        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug(
                "End turn for {PreviosPlayerId} and active player {CurrentPlayerId}",
                @event.PlayerId.Value,
                gameState.TurnAutomat.ActivePlayer.Value);
        }

        writer.Write(new TurnStartedEvent(@event.GameId, gameState.TurnAutomat.ActivePlayer));
    }
}