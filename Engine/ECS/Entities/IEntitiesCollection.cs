namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Provides entity lookup, enumeration, addition, removal, and component editing.
/// </summary>
public interface IEntitiesCollection : IReadOnlyEntitiesCollection
{
    /// <summary>
    /// Retrieves an entity and exposes its component editing operations.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    /// <returns>
    /// The entity or component of the requested type.
    /// </returns>
    public new EditableEntity<TEntity> Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    /// <summary>
    /// Adds entities, replacing entries with the same concrete type and identifier.
    /// </summary>
    /// <param name="entities">
    /// The entities to process.
    /// </param>
    public void Add(params IEnumerable<EntityBase> entities);

    /// <summary>
    /// Adds entities, replacing entries with the same concrete type and identifier.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <param name="entities">
    /// The entities to process.
    /// </param>
    public void Add<TEntity>(params IEnumerable<EditableEntity<TEntity>> entities)
        where TEntity : EntityBase;

    /// <summary>
    /// Removes the entity identified by its concrete type and identifier.
    /// </summary>
    /// <param name="entity">
    /// The entity to remove.
    /// </param>
    public void Remove(EntityBase entity);

    /// <summary>
    /// Enumerates editable entities of the requested concrete type.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <returns>
    /// The entities of the requested concrete type.
    /// </returns>
    public new IEnumerable<EditableEntity<TEntity>> Query<TEntity>()
        where TEntity : EntityBase;
}
