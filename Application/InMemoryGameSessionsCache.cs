using System.Collections.Concurrent;

using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Stores game sessions in a concurrent in-memory dictionary.
/// </summary>
public sealed class InMemoryGameSessionsCache : IGameSessionsCache
{
    private readonly ConcurrentDictionary<GameId, IGameSession> _sessions = [];

    /// <inheritdoc/>
    public async ValueTask AddAsync(IGameSession session, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(session);

        token.ThrowIfCancellationRequested();

        _sessions[session.GameId] = session;
    }

    /// <inheritdoc/>
    public async ValueTask<IGameSession?> FindAsync(
        GameId gameId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        return _sessions.TryGetValue(gameId, out var session)
            ? session
            : null;
    }

    /// <inheritdoc/>
    public async ValueTask RemoveByGameIdAsync(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _ = _sessions.Remove(gameId, out var session);

        session?.Dispose();
    }
}
