using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Controls the turn queue and its optional repeating player cycle.
/// </summary>
public interface ITurnAutomat : IReadOnlyTurnAutomat
{
    /// <summary>
    /// Disables replenishment of the turn queue while preserving the queued turns.
    /// </summary>
    public void DropCycle();

    /// <summary>
    /// Removes all queued turns after the first occurrence of the specified player.
    /// </summary>
    /// <param name="playerId">
    /// The player entity identifier whose following turns are removed.
    /// </param>
    public void DropQueueAfter(EntityId playerId);

    /// <summary>
    /// Removes the active turn and replenishes an exhausted queue when a cycle is configured.
    /// </summary>
    public void MoveNext();

    /// <summary>
    /// Replaces the repeating player cycle and resets the turn queue to that order.
    /// </summary>
    /// <param name="playerIds">
    /// The player entity identifiers in the repeating turn order.
    /// </param>
    public void SetCycle(IReadOnlyCollection<EntityId> playerIds);

    /// <summary>
    /// Inserts player turns immediately after the active turn, or initializes an empty queue.
    /// </summary>
    /// <param name="players">
    /// The player entity identifiers to insert after the active turn.
    /// </param>
    public void SetNext(params IReadOnlyCollection<EntityId> players);
}
