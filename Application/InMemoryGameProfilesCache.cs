using System.Collections.Concurrent;

using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application;

public sealed class InMemoryGameProfilesCache : IGameProfilesCache
{
    private readonly ConcurrentDictionary<GameTypeId, IGameProfile> _gameProfiles;

    private readonly IGameDescriptionRepository _descriptionRepository;

    public InMemoryGameProfilesCache(
        IGameDescriptionRepository sessionsCache,
        IEnumerable<KeyValuePair<GameTypeId, IGameProfile>> profiles)
    {
        ArgumentNullException.ThrowIfNull(sessionsCache);
        _descriptionRepository = sessionsCache;

        ArgumentNullException.ThrowIfNull(profiles);
        _gameProfiles = new(profiles);
    }

    public async ValueTask<IGameProfile> GetProfileAsync(
        GameId gameId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var description = await _descriptionRepository.FindAsync(gameId, token)
            ?? throw new InvalidOperationException(
                $"Session for game {gameId.Value} not found.");

        return await GetProfileAsync(description.GameTypeId, token);
    }

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
