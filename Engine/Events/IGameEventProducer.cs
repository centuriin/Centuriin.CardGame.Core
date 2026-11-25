using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

/// <summary>
/// Applies an initial event and drains all queued successor events.
/// </summary>
public interface IGameEventProducer
{
    /// <summary>
    /// Applies the initial event and queued successors, collecting them in application order.
    /// </summary>
    /// <param name="event">
    /// The domain or application event to process.
    /// </param>
    /// <returns>
    /// All applied events in their application order.
    /// </returns>
    public IReadOnlyList<IGameEvent> Produce(IGameEvent @event);
}
