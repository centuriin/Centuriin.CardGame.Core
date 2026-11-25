using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;

namespace Centuriin.CardGame.Core.Application.Initialization;

/// <summary>
/// Describes game initializer for game initialization before starting.
/// </summary>
public interface IGameInitializer
{
    /// <summary>
    /// Populates the initial entity context for a game.
    /// </summary>
    /// <param name="gameDescription">
    /// The game description containing players, type, and settings.
    /// </param>
    /// <param name="initialStateContext">
    /// The mutable context to populate with initial entities.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task InitializeAsync(
        GameDescription gameDescription,
        IInitialStateContext initialStateContext,
        CancellationToken token);
}