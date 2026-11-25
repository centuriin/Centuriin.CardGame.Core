using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Loading;

/// <summary>
/// Adds source entity instances of one type to the target game's entity collection.
/// </summary>
/// <typeparam name="TEntity">
/// The entity type.
/// </typeparam>
public sealed class EntityLoader<TEntity> : IEntityLoader
    where TEntity : EntityBase
{
    /// <inheritdoc/>
    public void Load(
        IReadOnlyEntitiesCollection entitiesCollection,
        IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(entitiesCollection);
        ArgumentNullException.ThrowIfNull(gameState);

        foreach (var entity in entitiesCollection.Query<TEntity>())
        {
            gameState.Entities.Add(entity);
        }
    }
}
