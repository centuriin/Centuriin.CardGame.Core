using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameStartupService : IGameStartupService
{
    private readonly IGameSessionFactory _gameSessionFactory;
    private readonly IGameSessionsRepository _sessionsRepository;
    private readonly IEnumerable<IGameLoader> _loaders;

    public GameStartupService(
        IGameSessionFactory gameSessionFactory,
        IGameSessionsRepository sessionsRepository,
        IEnumerable<IGameLoader> loaders)
    {
        ArgumentNullException.ThrowIfNull(sessionsRepository);
        _sessionsRepository = sessionsRepository;

        ArgumentNullException.ThrowIfNull(loaders);
        _loaders = loaders;

        ArgumentNullException.ThrowIfNull(gameSessionFactory);
        _gameSessionFactory = gameSessionFactory;
    }

    public async Task<IGame> StartupGameAsync(GameSetup setup, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        token.ThrowIfCancellationRequested();

        var session = await _gameSessionFactory.CreateAsync(setup, token);

        await _sessionsRepository.AddAsync(session, token);

        foreach (var loader in _loaders)
        {
            await loader.LoadAsync(setup, session.Game.State, token);
        }

        await session.Game.StartAsync(token);

        return session.Game;
    }
}
