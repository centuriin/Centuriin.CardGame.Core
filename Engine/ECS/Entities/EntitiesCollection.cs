using System.Collections;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

public sealed class EntitiesCollection : IEntitiesCollection
{
    private readonly Dictionary<Type, IDictionary> _entities = [];

    private EntitiesCollection(Dictionary<Type, IDictionary> entities)
    {
        _entities = entities.ToDictionary();
    }

    public EntitiesCollection() { }

    public bool IsEmpty => _entities.Count == 0;

    public void AddEntity<TEntity>(TEntity entity)
        where TEntity : EntityBase
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            _entities[typeof(TEntity)] = new Dictionary<EntityId, TEntity>()
            {
                { entity.Id, entity }
            };

            return;
        }

        ((Dictionary<EntityId, TEntity>)dict)[entity.Id] = entity;
    }

    public TEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase
    {
        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            throw new InvalidOperationException();
        }

        return ((IDictionary<EntityId, TEntity>)dict)[id];
    }

    IReadOnlyEntity IReadOnlyEntitiesCollection.Get<TEntity>(EntityId id) => 
        Get<TEntity>(id);

    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase
    {
        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            return [];
        }

        return (IEnumerable<TEntity>)dict!.Values;
    }

    IEnumerable<IReadOnlyEntity> IReadOnlyEntitiesCollection.Query<TEntity>() =>
        Query<TEntity>();

    public IEntitiesCollection ToCollection() => new EntitiesCollection(_entities);
}
