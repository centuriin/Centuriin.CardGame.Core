using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Dispatching;

/// <summary>
/// Applies a domain event to mutable game state.
/// </summary>
/// <typeparam name="TEvent">
/// The event type handled by this contract.
/// </typeparam>
public interface IConsumingSubscriber<TEvent>
    where TEvent : IGameEvent
{
    /// <summary>
    /// Handles a domain event using the state and capabilities supplied by the dispatcher.
    /// </summary>
    /// <param name="event">
    /// The domain or application event to process.
    /// </param>
    /// <param name="gameState">
    /// The game state used by the operation.
    /// </param>
    public void OnEvent(TEvent @event, IGameState gameState);
}