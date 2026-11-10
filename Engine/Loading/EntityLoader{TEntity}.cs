using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Loading;

public sealed class EntityLoader<TEntity> : IEntityLoader
    where TEntity : EntityBase
{
    public void Load(
        IReadOnlyEntitiesCollection entitiesCollection,
        IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(entitiesCollection);
        ArgumentNullException.ThrowIfNull(gameState);

        foreach (var entity in entitiesCollection.Query<TEntity>())
        {
            gameState.Entities.AddEntity(entity);
        }
    }
}
