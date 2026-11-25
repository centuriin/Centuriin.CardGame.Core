namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Provides entity lookup and enumeration without exposing editing operations.
/// </summary>
public interface IReadOnlyEntitiesCollection : IEnumerable<EntityBase>
{
    /// <summary>
    /// Gets whether the collection contains no entries.
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    /// Retrieves an entity by its type and identifier.
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
    public TEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    /// <summary>
    /// Enumerates entities of the requested concrete type.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <returns>
    /// The entities of the requested concrete type.
    /// </returns>
    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase;
}
