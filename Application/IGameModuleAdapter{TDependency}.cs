using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Resolves a dependency from the module associated with a game type.
/// </summary>
/// <typeparam name="TDependency">
/// The game-specific dependency type to resolve.
/// </typeparam>
public interface IGameModuleAdapter<TDependency>
    where TDependency : notnull
{
    /// <summary>
    /// Resolves the module dependency associated with a game type.
    /// </summary>
    /// <param name="gameTypeId">
    /// The identifier of the game type.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The dependency resolved from the selected game module.
    /// </returns>
    public ValueTask<TDependency> GetDependencyAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}