using System.Collections.Concurrent;

using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Stores registered profiles in memory and resolves game types from descriptions.
/// </summary>
public sealed class InMemoryGameProfilesCache : IGameProfilesCache
{
    private readonly ConcurrentDictionary<GameTypeId, IGameProfile> _gameProfiles;

    private readonly IGameDescriptionRepository _descriptionRepository;

    /// <summary>
    /// Initializes a new <see cref="InMemoryGameProfilesCache"/> instance.
    /// </summary>
    /// <param name="descriptionRepository">
    /// The repository used to retrieve game descriptions.
    /// </param>
    /// <param name="profiles">
    /// The profiles indexed by game type identifier.
    /// </param>
    public InMemoryGameProfilesCache(
        IGameDescriptionRepository descriptionRepository,
        IEnumerable<KeyValuePair<GameTypeId, IGameProfile>> profiles)
    {
        ArgumentNullException.ThrowIfNull(descriptionRepository);
        _descriptionRepository = descriptionRepository;

        ArgumentNullException.ThrowIfNull(profiles);
        _gameProfiles = new(profiles);
    }

    /// <inheritdoc/>
    public async ValueTask<IGameProfile> GetProfileAsync(
        GameId gameId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var description = await _descriptionRepository.FindAsync(gameId, token)
            ?? throw new InvalidOperationException(
                $"Description for game {gameId.Value} not found.");

        return await GetProfileAsync(description.GameTypeId, token);
    }

    /// <inheritdoc/>
    public async ValueTask<IGameProfile> GetProfileAsync(
        GameTypeId gameTypeId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (!_gameProfiles.TryGetValue(gameTypeId, out var profile))
        {
            throw new InvalidOperationException(
                $"Game profile for type {gameTypeId.Value} not found.");
        }

        return profile;
    }
}
