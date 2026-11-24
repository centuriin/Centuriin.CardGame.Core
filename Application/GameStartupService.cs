using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application;

public sealed class GameStartupService : IGameStartupService
{
    private readonly IGameSessionsCache _sessionsCache;
    private readonly IGameSessionFactory _sessionFactory;

    public GameStartupService(
        IGameSessionsCache sessionsCache,
        IGameSessionFactory sessionFactory)
    {
        ArgumentNullException.ThrowIfNull(sessionsCache);
        _sessionsCache = sessionsCache;

        ArgumentNullException.ThrowIfNull(sessionFactory);
        _sessionFactory = sessionFactory;
    }

    public async Task<IGameSession> StartupGameAsync(
        GameDescription description, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(description);

        token.ThrowIfCancellationRequested();

        if (await _sessionsCache.FindAsync(description.GameId, token) is { })
        {
            throw new InvalidOperationException(
                $"Game session {description.GameId.Value} already exists.");
        }

        var session = await _sessionFactory.CreateAsync(description, token);

        try
        {
            await _sessionsCache.AddAsync(session, token);

            await session.StartAsync(token);

            return session;
        }
        catch
        {
            await _sessionsCache.RemoveByGameIdAsync(session.GameId, token);

            throw;
        }
    }
}
