using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.Storage.Repositories;
using Centuriin.CardGame.Core.Engine.World;

namespace Centuriin.CardGame.Core.Engine;

public sealed class GameStartupService : IGameStartupService
{
    private readonly IGameSessionFactory _sessionFactory;
    private readonly IGameSessionsRepository _sessionsRepository;


    public GameStartupService(
        IGameSessionFactory sessionFactory,
        IGameSessionsRepository sessionsRepository)
    {
        ArgumentNullException.ThrowIfNull(sessionFactory);
        _sessionFactory = sessionFactory;

        ArgumentNullException.ThrowIfNull(sessionsRepository);
        _sessionsRepository = sessionsRepository;
    }

    public async Task<IGameSession> StartupGameAsync(GameSetup setup, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        token.ThrowIfCancellationRequested();

        var session = await _sessionFactory.CreateAsync(setup, token);

        try
        {
            await _sessionsRepository.AddAsync(session, token);

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
