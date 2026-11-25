using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Root.State;

/// <summary>
/// Exposes game state without editing operations.
/// </summary>
/// <remarks>
/// This is a view of live state; it is not an independent immutable snapshot.
/// </remarks>
public interface IReadOnlyGameState
{
    /// <summary>
    /// Gets the current game lifecycle status.
    /// </summary>
    public GameStatus Status { get; }

    /// <summary>
    /// Gets the current turn order without exposing queue editing operations.
    /// </summary>
    public IReadOnlyTurnAutomat TurnAutomat { get; }

    /// <summary>
    /// Gets the game's entities without exposing editing operations.
    /// </summary>
    public IReadOnlyEntitiesCollection Entities { get; }
}
