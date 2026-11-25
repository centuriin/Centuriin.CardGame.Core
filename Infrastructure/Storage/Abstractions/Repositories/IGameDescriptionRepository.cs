using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

/// <summary>
/// Stores and retrieves game descriptions.
/// </summary>
public interface IGameDescriptionRepository
{
    /// <summary>
    /// Finds a game description by its identifier.
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
    public Task<GameDescription?> FindAsync(GameId gameId, CancellationToken token);

    /// <summary>
    /// Stores a game description.
    /// </summary>
    /// <param name="gameDescription">
    /// The game description containing players, type, and settings.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task AddAsync(GameDescription gameDescription, CancellationToken token);
}
