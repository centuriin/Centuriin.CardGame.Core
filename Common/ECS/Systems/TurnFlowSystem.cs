using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.ECS.Systems;

public sealed class TurnFlowSystem :
    ISystem,
    IConsumingSubscriber<TurnFlowDefinedEvent>,
    IProducingSubscriber<TurnFlowDefinedEvent>,
    IConsumingSubscriber<TurnEndedEvent>,
    IProducingSubscriber<TurnEndedEvent>
{
    public void OnEvent(TurnFlowDefinedEvent @event, IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(gameState);

        if (@event.IsCycled)
        {
            gameState.TurnAutomat.SetCycle(@event.InitialPlayerTurnsOrder);
        }
        else
        {
            gameState.TurnAutomat.SetNext(@event.InitialPlayerTurnsOrder);
        }
    }

    public void OnEvent(
        TurnFlowDefinedEvent @event,
        IReadOnlyGameState readOnlyGameState,
        IGameEventBusWriter eventBusWriter)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(readOnlyGameState);
        ArgumentNullException.ThrowIfNull(eventBusWriter);

        eventBusWriter.Write(
            new TurnStartedEvent(
                @event.GameId,
                readOnlyGameState.TurnAutomat.ActivePlayer));
    }

    public void OnEvent(TurnEndedEvent @event, IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(gameState);

        gameState.TurnAutomat.MoveNext();
    }

    public void OnEvent(
        TurnEndedEvent @event,
        IReadOnlyGameState readOnlyGameState,
        IGameEventBusWriter eventBusWriter)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(readOnlyGameState);
        ArgumentNullException.ThrowIfNull(eventBusWriter);

        eventBusWriter.Write(
            new TurnStartedEvent(
                @event.GameId,
                readOnlyGameState.TurnAutomat.ActivePlayer));
    }
}