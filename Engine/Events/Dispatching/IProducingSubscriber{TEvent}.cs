using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Dispatching;

/// <summary>
/// Produces successor events from a domain event and the current game state.
/// </summary>
/// <typeparam name="TEvent">
/// The event type handled by this contract.
/// </typeparam>
public interface IProducingSubscriber<TEvent>
    where TEvent : IGameEvent
{
    /// <summary>
    /// Handles a domain event using the state and capabilities supplied by the dispatcher.
    /// </summary>
    /// <param name="event">
    /// The domain or application event to process.
    /// </param>
    /// <param name="readOnlyGameState">
    /// The current game state available for reading.
    /// </param>
    /// <param name="eventBusWriter">
    /// The queue writer used to publish successor events.
    /// </param>
    public void OnEvent(
        TEvent @event,
        IReadOnlyGameState readOnlyGameState,
        IGameEventBusWriter eventBusWriter);
}