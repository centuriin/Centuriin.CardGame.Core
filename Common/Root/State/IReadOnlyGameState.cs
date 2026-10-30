using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common.Root.State;

public interface IReadOnlyGameState
{
    /// <summary>
    /// Gets entity copy from mutable state.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Entity type.
    /// </typeparam>
    /// <param name="id">
    /// Entity id.
    /// </param>
    /// <returns>
    /// Entity copy.
    /// </returns>
    public EntityCopy<TEntity> Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    /// <summary>
    /// Query entity copies.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Entity type.
    /// </typeparam>
    /// <returns>
    /// Entity copy.
    /// </returns>
    public IEnumerable<EntityCopy<TEntity>> Query<TEntity>()
        where TEntity : EntityBase;
}
