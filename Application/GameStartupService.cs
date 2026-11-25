using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Creates, caches, and starts game sessions.
/// </summary>
public sealed class GameStartupService : IGameStartupService
{
    private readonly IGameSessionsCache _sessionsCache;
    private readonly IGameSessionFactory _sessionFactory;

    /// <summary>
    /// Initializes a new <see cref="GameStartupService"/> instance.
    /// </summary>
    /// <param name="sessionsCache">
    /// The cache containing active game sessions.
    /// </param>
    /// <param name="sessionFactory">
    /// The factory used to create game sessions.
    /// </param>
    public GameStartupService(
        IGameSessionsCache sessionsCache,
        IGameSessionFactory sessionFactory)
    {
        ArgumentNullException.ThrowIfNull(sessionsCache);
        _sessionsCache = sessionsCache;

        ArgumentNullException.ThrowIfNull(sessionFactory);
        _sessionFactory = sessionFactory;
    }

    /// <inheritdoc/>
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
