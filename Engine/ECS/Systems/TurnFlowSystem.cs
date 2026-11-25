using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.ECS.Systems;

/// <summary>
/// Applies turn order changes and produces notifications for the next active player.
/// </summary>
public sealed class TurnFlowSystem :
    ISystem,
    IConsumingSubscriber<TurnFlowDefinedEvent>,
    IProducingSubscriber<TurnFlowDefinedEvent>,
    IConsumingSubscriber<TurnEndedEvent>,
    IProducingSubscriber<TurnEndedEvent>
{
    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public void OnEvent(TurnEndedEvent @event, IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(gameState);

        gameState.TurnAutomat.MoveNext();
    }

    /// <inheritdoc/>
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