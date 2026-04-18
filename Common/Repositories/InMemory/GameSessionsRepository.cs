using System.Collections.Concurrent;

using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Repositories.InMemory;

public sealed class GameSessionsRepository : IGameSessionsRepository
{
    private readonly ConcurrentDictionary<GameId, IGameSession> _sessions = [];

    public async ValueTask<IGame> GetGameById(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (!_sessions.TryGetValue(gameId, out var session))
        {
            throw new InvalidOperationException();
        }

        return session.Game;
    }

    public ValueTask AddAsync(IGameSession session, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(session);

        token.ThrowIfCancellationRequested();

        if (!_sessions.TryAdd(session.Game.GameId, session))
        {
            throw new InvalidOperationException();
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveByGameIdAsync(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (_sessions.TryRemove(gameId, out var session))
        {
            session.Dispose();
        }

        return ValueTask.CompletedTask;
    }
}