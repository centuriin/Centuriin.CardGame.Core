using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Exposes the active player and the remaining turn queue.
/// </summary>
public interface IReadOnlyTurnAutomat
{
    /// <summary>
    /// Gets the player entity identifier at the front of the turn queue.
    /// </summary>
    public EntityId ActivePlayer { get; }

    /// <summary>
    /// Gets whether the configured player order repeats when the queue is exhausted.
    /// </summary>
    public bool IsCycled { get; }

    /// <summary>
    /// Enumerates the currently queued player turns, starting with the active player.
    /// </summary>
    /// <returns>
    /// The currently queued player entity identifiers in turn order.
    /// </returns>
    public IEnumerable<EntityId> GetEnumerable();
}
