using System.Collections.Concurrent;

using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Stores registered profiles in memory and resolves game types from descriptions.
/// </summary>
public sealed class InMemoryGameProfilesCache : IGameModulesCache
{
    private readonly ConcurrentDictionary<GameTypeId, IGameModule> _gameModules;

    /// <summary>
    /// Initializes a new <see cref="InMemoryGameProfilesCache"/> instance.
    /// </summary>
    /// <param name="modules">
    /// The profiles indexed by game type identifier.
    /// </param>
    public InMemoryGameProfilesCache(
        IEnumerable<KeyValuePair<GameTypeId, IGameModule>> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);
        _gameModules = new(modules);
    }

    /// <inheritdoc/>
    public async ValueTask<IGameModule> GetModuleAsync(
        GameTypeId gameTypeId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (!_gameModules.TryGetValue(gameTypeId, out var module))
        {
            throw new InvalidOperationException(
                $"Game module for type {gameTypeId.Value} not found.");
        }

        return module;
    }
}
