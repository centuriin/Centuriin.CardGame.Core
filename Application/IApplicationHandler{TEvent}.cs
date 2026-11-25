namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Handles an application event asynchronously.
/// </summary>
/// <typeparam name="TEvent">
/// The event type handled by this contract.
/// </typeparam>
public interface IApplicationHandler<in TEvent>
{
    /// <summary>
    /// Handles an application notification.
    /// </summary>
    /// <param name="event">
    /// The domain or application event to process.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task HandleAsync(TEvent @event, CancellationToken token);
}
