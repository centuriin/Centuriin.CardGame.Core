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
    public ValueTask<TDependency> ResolveSingleAsync(
        GameTypeId gameTypeId,
        CancellationToken token);

    public ValueTask<IEnumerable<TDependency>> ResolveCollectionAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}