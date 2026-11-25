using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

/// <summary>
/// Reads queued domain events in first-in, first-out order.
/// </summary>
public interface IGameEventBusReader
{
    /// <summary>
    /// Attempts to remove the next queued event.
    /// </summary>
    /// <param name="event">
    /// The removed event, or <see langword="null"/> when the queue is empty.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an event was removed; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryRead(out IGameEvent @event);
}
