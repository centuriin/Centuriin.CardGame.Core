using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application.Modulation;

/// <summary>
/// Resolves a dependency from the module associated with a game type.
/// </summary>
/// <typeparam name="TDependency">
/// The game-specific dependency type to resolve.
/// </typeparam>
public interface IModulable<TDependency>
    where TDependency : notnull
{
    /// <summary>
    /// Resolves a single dependency registered under the module's key.
    /// </summary>
    /// <param name="gameTypeId">
    /// The game type used to select the module.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The dependency registered for the selected module.
    /// </returns>
    public ValueTask<TDependency> ResolveSingleAsync(
        GameTypeId gameTypeId,
        CancellationToken token);

    /// <summary>
    /// Resolves all dependencies registered under the module's key.
    /// </summary>
    /// <param name="gameTypeId">
    /// The game type used to select the module.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The dependencies in registration order.
    /// </returns>
    public ValueTask<IEnumerable<TDependency>> ResolveCollectionAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}
