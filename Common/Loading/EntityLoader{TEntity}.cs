using Centuriin.CardGame.Core.Common.Context;
using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Loading;

public sealed class EntityLoader<TEntity> : IGameLoader
    where TEntity : EntityBase
{
    private readonly IGameState _gameState;
    private readonly IGameContextAccessor<IInitialStateContext> _contextAccessor;

    public EntityLoader(
        IGameState gameState,
        IGameContextAccessor<IInitialStateContext> contextAccessor)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(contextAccessor);
        _contextAccessor = contextAccessor;
    }

    public async Task LoadAsync(GameId gameId, CancellationToken token)
    {
        var initialStateContext = await _contextAccessor.GetContextAsync(gameId, token);

        var entities = initialStateContext.GetEntities<TEntity>();

        foreach (var entity in entities)
        {
            _gameState.AddEntity(entity);
        }
    }
}
