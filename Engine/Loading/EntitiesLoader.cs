using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Loading;

public sealed class EntitiesLoader : IGameLoader
{
    private readonly IGameState _gameState;
    private readonly IGameContextAccessor<IReadOnlyInitialStateContext> _gameContextAccessor;

    private readonly IEnumerable<IEntityLoader> _entityLoaders;

    public EntitiesLoader(
        IGameState gameState,
        IGameContextAccessor<IReadOnlyInitialStateContext> gameContextAccessor,
        IEnumerable<IEntityLoader> entityLoaders)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(gameContextAccessor);
        _gameContextAccessor = gameContextAccessor;

        ArgumentNullException.ThrowIfNull(entityLoaders);
        _entityLoaders = entityLoaders;
    }

    public async Task LoadAsync(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var context = await _gameContextAccessor.GetContextAsync(gameId, token);

        foreach (var loader in _entityLoaders)
        {
            loader.Load(context.Entities, _gameState);
        }
    }
}
