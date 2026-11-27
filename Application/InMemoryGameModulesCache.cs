using System.Collections.Concurrent;

using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Stores discovered module instances and resolves the host's current game type mappings.
/// </summary>
public sealed class InMemoryGameModulesCache : IGameModulesCache
{
    private readonly ConcurrentDictionary<string, IGameModule> _gameModules;
    private readonly IGameTypeModulesRepository _gameTypes;

    /// <summary>
    /// Initializes a new <see cref="InMemoryGameModulesCache"/> instance.
    /// </summary>
    /// <param name="modules">
    /// The module instances discovered at startup.
    /// </param>
    /// <param name="gameTypes">
    /// The repository containing the host's current game type mappings.
    /// </param>
    public InMemoryGameModulesCache(
        IEnumerable<IGameModule> modules,
        IGameTypeModulesRepository gameTypes)
    {
        ArgumentNullException.ThrowIfNull(modules);
        _gameModules = new(
            modules.Select(module => KeyValuePair.Create(module.Key, module)),
            StringComparer.Ordinal);

        ArgumentNullException.ThrowIfNull(gameTypes);
        _gameTypes = gameTypes;
    }

    /// <inheritdoc/>
    public async ValueTask<IGameModule> GetModuleAsync(
        GameTypeId gameTypeId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var moduleKey = await _gameTypes.FindModuleKeyAsync(gameTypeId, token)
            ?? throw new InvalidOperationException($"Game type {gameTypeId.Value} is not registered.");

        if (!_gameModules.TryGetValue(moduleKey, out var module))
        {
            throw new InvalidOperationException(
                $"Game module {moduleKey} for type {gameTypeId.Value} is not installed.");
        }

        return module;
    }
}
