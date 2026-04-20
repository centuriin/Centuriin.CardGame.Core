using System.Collections.Concurrent;

using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Repositories.InMemory;

public sealed class GameSessionsRepository : IGameSessionsRepository
{
    private readonly ConcurrentDictionary<GameId, IGameSession> _sessions = [];
    private bool _disposed;

    /// <inheritdoc/>
    public async ValueTask<IGame> GetGameById(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!_sessions.TryGetValue(gameId, out var session))
        {
            throw new InvalidOperationException();
        }

        return session.Game;
    }

    /// <inheritdoc/>
    public ValueTask AddAsync(IGameSession session, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(session);

        token.ThrowIfCancellationRequested();

        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!_sessions.TryAdd(session.Game.GameId, session))
        {
            throw new InvalidOperationException();
        }

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public ValueTask RemoveByGameIdAsync(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_sessions.TryRemove(gameId, out var session))
        {
            session.Dispose();
        }

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (!_disposed)
        {
            foreach (var session in _sessions.Values)
            {
                session.Dispose();
            }
            _sessions.Clear();
        }

        _disposed = true;
    }
}