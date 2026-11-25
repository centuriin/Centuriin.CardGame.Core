using System.Collections;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Stores entities by their concrete type and identifier.
/// </summary>
public sealed class EntitiesCollection : IEntitiesCollection
{
    private readonly Dictionary<Type, IDictionary> _entities = [];

    /// <inheritdoc/>
    public bool IsEmpty => _entities.Values.All(entities => entities.Count == 0);

    /// <summary>
    /// Creates an empty entity collection.
    /// </summary>
    public EntitiesCollection() { }

    /// <summary>
    /// Creates a collection containing the supplied entity instances.
    /// </summary>
    /// <param name="entities">
    /// The entities to process.
    /// </param>
    /// <remarks>
    /// The entity instances are shared with the source; they are not cloned.
    /// </remarks>
    public EntitiesCollection(IEnumerable<EntityBase> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        _entities = entities.GroupBy(k => k.GetType())
            .ToDictionary(
                g => g.Key,
                g => (IDictionary)g.ToDictionary(e => e.Id));
    }

    /// <inheritdoc/>
    public void Add<TEntity>(params IEnumerable<EditableEntity<TEntity>> entities)
        where TEntity : EntityBase
        => Add(entities.Select(x => x.Entity));

    /// <inheritdoc/>
    public void Add(params IEnumerable<EntityBase> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        foreach (var entity in entities)
        {
            var entityType = entity.GetType();

            if (_entities.TryGetValue(entityType, out var value))
            {
                ((IDictionary<EntityId, EntityBase>)value)[entity.Id] = entity;
            }
            else
            {
                _entities[entityType] = new Dictionary<EntityId, EntityBase>()
                {
                    { entity.Id, entity },
                };
            }
        }
    }

    /// <inheritdoc/>
    public void Remove(EntityBase entity) => 
        _ = ((IDictionary<EntityId, EntityBase>)_entities[entity.GetType()])
            .Remove(entity.Id);

    /// <inheritdoc/>
    public EditableEntity<TEntity> Get<TEntity>(EntityId id)
        where TEntity : EntityBase
    {
        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            throw new InvalidOperationException();
        }

        return new((TEntity)((IDictionary<EntityId, EntityBase>)dict)[id]);
    }

    /// <inheritdoc/>
    TEntity IReadOnlyEntitiesCollection.Get<TEntity>(EntityId id)
    {
        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            throw new InvalidOperationException();
        }

        return (TEntity)((IDictionary<EntityId, EntityBase>)dict)[id];
    }

    /// <inheritdoc/>
    public IEnumerable<EditableEntity<TEntity>> Query<TEntity>()
        where TEntity : EntityBase
    {
        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            return [];
        }

        return ((IDictionary<EntityId, EntityBase>)dict).Values
            .Cast<TEntity>()
            .Select(x => new EditableEntity<TEntity>(x));
    }

    /// <inheritdoc/>
    IEnumerable<TEntity> IReadOnlyEntitiesCollection.Query<TEntity>()
    {
        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            return [];
        }

        return ((IDictionary<EntityId, EntityBase>)dict).Values.Cast<TEntity>();
    }

    /// <inheritdoc/>
    public IEnumerator<EntityBase> GetEnumerator() => 
        _entities.Values
            .SelectMany(x => x.Values.OfType<EntityBase>())
            .GetEnumerator();
    
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
