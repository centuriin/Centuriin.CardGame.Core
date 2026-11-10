using Application;

namespace Centuriin.CardGame.Core.Engine;

public sealed class GameStartupService : IGameStartupService
{
    private readonly IGameSessionFactory _sessionFactory;


    public GameStartupService(
        IGameSessionFactory sessionFactory)
    {
        ArgumentNullException.ThrowIfNull(sessionFactory);
        _sessionFactory = sessionFactory;
    }

    public async Task<IGameSession> StartupGameAsync(GameSetup setup, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        token.ThrowIfCancellationRequested();

        var session = await _sessionFactory.CreateAsync(setup, token);

        try
        {
            await session.StartAsync(token);

            return session;
        }
        catch
        {
            session.Dispose();
            throw;
        }
    }
}
