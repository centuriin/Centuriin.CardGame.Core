using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Stores active game sessions by game identifier.
/// </summary>
public interface IGameSessionsCache
{
    /// <summary>
    /// Finds a cached game session by its identifier.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The matching value, or <see langword="null"/> when it is not found.
    /// </returns>
    public ValueTask<IGameSession?> FindAsync(GameId gameId, CancellationToken token);

    /// <summary>
    /// Adds or replaces the cached session for a game identifier.
    /// </summary>
    /// <param name="session">
    /// The game session associated with the operation.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A value task that completes when the operation finishes.
    /// </returns>
    public ValueTask AddAsync(IGameSession session, CancellationToken token);

    /// <summary>
    /// Removes and disposes the cached session, when present.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A value task that completes when the operation finishes.
    /// </returns>
    public ValueTask RemoveByGameIdAsync(GameId gameId, CancellationToken token);
}
