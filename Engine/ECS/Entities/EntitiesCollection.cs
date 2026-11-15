using System.Collections;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

public sealed class EntitiesCollection : IEntitiesCollection
{
    private readonly Dictionary<Type, IDictionary> _entities = [];

    public EntitiesCollection() { }

    public EntitiesCollection(IEnumerable<EntityBase> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        _entities = entities.GroupBy(k => k.GetType())
            .ToDictionary(
                g => g.Key,
                g => (IDictionary)g.ToDictionary(e => e.Id));
    }

    public bool IsEmpty => _entities.Count == 0;

    public void Add<TEntity>(TEntity entity)
        where TEntity : EntityBase
    {
        ArgumentNullException.ThrowIfNull(entity);

        var entityType = entity.GetType();

        if (!_entities.TryGetValue(entityType, out var dict))
        {
            _entities[entityType] = new Dictionary<EntityId, TEntity>()
            {
                { entity.Id, entity }
            };

            return;
        }

        var entityDict = (Dictionary<EntityId, TEntity>)dict;

        if (entityDict.ContainsKey(entity.Id))
        {
            throw new InvalidOperationException(
                $"Entity {entityType.Name} with id {entity.Id.Value} already exists.");
        }

        entityDict[entity.Id] = entity;
    }

    public void Replace<TEntity>(TEntity replacement)
        where TEntity : EntityBase
    {
        ArgumentNullException.ThrowIfNull(replacement);

        var entityType = replacement.GetType();

        if (!_entities.TryGetValue(entityType, out var dict)
            || !((Dictionary<EntityId, TEntity>)dict).ContainsKey(replacement.Id))
        {
            throw new InvalidOperationException(
                $"Entity {entityType.Name} with id {replacement.Id.Value} not found.");
        }

        ((Dictionary<EntityId, TEntity>)dict)[replacement.Id] = replacement;
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

    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase
    {
        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            return [];
        }

        return (IEnumerable<TEntity>)dict!.Values;
    }

    public IEnumerator<EntityBase> GetEnumerator() => 
        _entities.Values
            .SelectMany(x => x.Values.OfType<EntityBase>())
            .GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
