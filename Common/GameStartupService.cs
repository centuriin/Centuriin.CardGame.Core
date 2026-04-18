using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameStartupService : IGameStartupService
{
    private readonly IEnumerable<IGameLoader> _loaders;
    private readonly IGameFactory _gameFactory;

    public GameStartupService(IEnumerable<IGameLoader> loaders, IGameFactory gameFactory)
    {
        ArgumentNullException.ThrowIfNull(loaders);
        _loaders = loaders;

        ArgumentNullException.ThrowIfNull(gameFactory);
        _gameFactory = gameFactory;
    }

    public async Task<IGame> StartupGameAsync(GameSetup setup, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        token.ThrowIfCancellationRequested();

        var game = _gameFactory.Create(setup);

        foreach (var loader in _loaders)
        {
            await loader.LoadAsync(setup, game.State, token);
        }

        await game.ApplyAsync(new GameStartedEvent(game.GameId), token);

        return game;
    }
}
