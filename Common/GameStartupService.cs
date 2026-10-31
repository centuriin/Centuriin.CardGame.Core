using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Storage.Repositories;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common;

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
            return session;
        }
        catch
        {
            session.Dispose();
            throw;
        }
    }
}
