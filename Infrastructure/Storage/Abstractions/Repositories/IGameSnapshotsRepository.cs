using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

/// <summary>
/// Stores and retrieves game snapshots and initial entity state.
/// </summary>
public interface IGameSnapshotsRepository
{
    /// <summary>
    /// Stores or replaces the snapshot of the supplied game.
    /// </summary>
    /// <param name="game">
    /// The game instance associated with the operation.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task AddOrUpdateSnapshotAsync(IGame game, CancellationToken token);

    /// <summary>
    /// Finds a stored snapshot by game identifier.
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
    public Task<IGameSnapshot?> FindAsync(GameId gameId, CancellationToken token);

    /// <summary>
    /// Stores the initial entity state for a game.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="gameTypeId">
    /// The identifier of the game type.
    /// </param>
    /// <param name="stateContext">
    /// The initial entity state to store.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task AddAsync(
        GameId gameId,
        GameTypeId gameTypeId,
        IReadOnlyInitialStateContext stateContext,
        CancellationToken token);
}