using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Stores the host's mapping from game types to module instances.
/// </summary>
public sealed class InMemoryGameModulesCache : IGameModulesCache
{
    private readonly Dictionary<GameTypeId, IGameModule> _gameModules;

    /// <summary>
    /// Initializes a new <see cref="InMemoryGameModulesCache"/> instance.
    /// </summary>
    /// <param name="modules">
    /// The game type mappings supplied by the host.
    /// </param>
    public InMemoryGameModulesCache(
        IEnumerable<KeyValuePair<GameTypeId, IGameModule>> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);
        _gameModules = new(modules);
    }

    /// <inheritdoc/>
    public ValueTask<IGameModule> GetModuleAsync(
        GameTypeId gameTypeId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (!_gameModules.TryGetValue(gameTypeId, out var module))
        {
            throw new InvalidOperationException(
                $"Game module for type {gameTypeId.Value} not found.");
        }

        return ValueTask.FromResult(module);
    }
}
