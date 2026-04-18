using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameStartupService : IGameStartupService
{
    private readonly IGameSessionsRepository _sessionsRepository;
    private readonly IEnumerable<IGameLoader> _loaders;
    private readonly IGameSessionFactory _gameFactory;

    public GameStartupService(
        IGameSessionsRepository sessionsRepository,
        IEnumerable<IGameLoader> loaders,
        IGameSessionFactory gameFactory)
    {
        ArgumentNullException.ThrowIfNull(sessionsRepository);
        _sessionsRepository = sessionsRepository;

        ArgumentNullException.ThrowIfNull(loaders);
        _loaders = loaders;

        ArgumentNullException.ThrowIfNull(gameFactory);
        _gameFactory = gameFactory;
    }

    public async Task<IGame> StartupGameAsync(GameSetup setup, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        token.ThrowIfCancellationRequested();

        var session = _gameFactory.Create(setup);

        await _sessionsRepository.AddAsync(session, token);

        foreach (var loader in _loaders)
        {
            await loader.LoadAsync(setup, session.Game.State, token);
        }

        await session.Game.ApplyAsync(new GameStartedEvent(session.Game.GameId), token);

        return session.Game;
    }
}
