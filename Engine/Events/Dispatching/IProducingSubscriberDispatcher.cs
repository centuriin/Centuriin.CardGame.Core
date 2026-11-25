using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events.Dispatching;

/// <summary>
/// Delivers domain events to subscribers that produce successor events.
/// </summary>
public interface IProducingSubscriberDispatcher
{
    /// <summary>
    /// Delivers the event to all matching subscribers in registration order.
    /// </summary>
    /// <param name="event">
    /// The domain or application event to process.
    /// </param>
    public void Publish(IGameEvent @event);

    /// <summary>
    /// Adds an event subscriber to the dispatcher.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The event type handled by this contract.
    /// </typeparam>
    /// <param name="subscriber">
    /// The event subscriber to register or remove.
    /// </param>
    public void Register<TEvent>(IProducingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;

    /// <summary>
    /// Removes an event subscriber from the dispatcher.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The event type handled by this contract.
    /// </typeparam>
    /// <param name="subscriber">
    /// The event subscriber to register or remove.
    /// </param>
    public void Unregister<TEvent>(IProducingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;
}