using System.Collections;

using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common;

public sealed class EntitiesCollection : IEntitiesCollection
{
    private readonly Dictionary<Type, IDictionary> _entities = [];

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

    TEntity IReadOnlyEntitiesCollection.Get<TEntity>(EntityId id) =>
        (TEntity)Get<TEntity>(id).Copy();

    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase
    {
        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            return [];
        }

        return (IEnumerable<TEntity>)dict!.Values;
    }

    IEnumerable<TEntity> IReadOnlyEntitiesCollection.Query<TEntity>() =>
        Query<TEntity>().Select(x => (TEntity)x.Copy());

    public IReadOnlyEntitiesCollection AsReadOnly() => this;
}
