using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

/// <summary>
/// Applies a recorded event without generating successor events.
/// </summary>
public interface IGameEventApplier
{
    /// <summary>
    /// Applies a recorded event to game state without producing successor events.
    /// </summary>
    /// <param name="event">
    /// The domain or application event to process.
    /// </param>
    public void Apply(IGameEvent @event);
}