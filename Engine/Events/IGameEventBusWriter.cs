using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

/// <summary>
/// Queues domain events for subsequent processing.
/// </summary>
public interface IGameEventBusWriter
{
    /// <summary>
    /// Appends an event to the end of the queue.
    /// </summary>
    /// <param name="event">
    /// The domain or application event to process.
    /// </param>
    public void Write(IGameEvent @event);
}