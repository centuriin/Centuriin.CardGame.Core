namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Delivers application events to their registered handlers.
/// </summary>
/// <typeparam name="TEvent">
/// The event type handled by this contract.
/// </typeparam>
public interface IApplicationDispatcher<TEvent>
{
    /// <summary>
    /// Delivers an event to the registered application handlers.
    /// </summary>
    /// <typeparam name="T">
    /// The concrete application event type.
    /// </typeparam>
    /// <param name="event">
    /// The domain or application event to process.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    /// <remarks>
    /// The generic event type must match the event's concrete runtime type.
    /// </remarks>
    public Task PublishAsync<T>(T @event, CancellationToken token)
        where T : TEvent;
}
