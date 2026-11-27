using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Application.Initialization;

/// <summary>
/// Describes game initializer for game initialization before starting.
/// </summary>
public interface IGameInitializer
{
    /// <summary>
    /// Populates the initial entity context for a game.
    /// </summary>
    /// <param name="description">
    /// The game description containing players, type, and settings.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The initialized state of the new game.
    /// </returns>
    public Task<IInitialStateContext> InitializeAsync(
        GameDescription description,
        CancellationToken token);

    /// <summary>
    /// Restores the initial entity context from a saved game snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// The snapshot containing the game identifier and entities.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The initial state restored from the snapshot.
    /// </returns>
    public Task<IInitialStateContext> InitializeAsync(
        IGameSnapshot snapshot,
        CancellationToken token);
}
