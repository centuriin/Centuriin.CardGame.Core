using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Loading;

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
