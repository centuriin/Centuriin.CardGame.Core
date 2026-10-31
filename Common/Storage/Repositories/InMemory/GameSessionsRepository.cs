using System.Collections.Concurrent;

using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Storage.Repositories;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories.InMemory;

public sealed class GameSessionsRepository : IGameSessionsRepository
{
    private readonly ConcurrentDictionary<GameId, IGameSession> _sessions = [];
    private bool _disposed;

    /// <inheritdoc/>
    public ValueTask<IGameSession> GetSessionByIdAsync(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!_sessions.TryGetValue(gameId, out var session))
        {
            throw new InvalidOperationException();
        }

        return ValueTask.FromResult(session);
    }

    /// <inheritdoc/>
    public ValueTask AddAsync(IGameSession session, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(session);

        token.ThrowIfCancellationRequested();

        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!_sessions.TryAdd(session.Id, session))
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
