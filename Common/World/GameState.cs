using System.Collections;

using Centuriin.CardGame.Core.Common.Entities;

namespace Centuriin.CardGame.Core.Common.World;

public sealed class GameState : IGameState
{
    private readonly Dictionary<Type, IDictionary> _entities = [];

    public ITurnAutomat TurnAutomat { get; }

    public GameState(ITurnAutomat turnAutomat)
    {
        ArgumentNullException.ThrowIfNull(turnAutomat);
        TurnAutomat = turnAutomat;
    }

    public void AddEntity<TEntity>(TEntity entity)
        where TEntity : EntityBase
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            _entities[typeof(TEntity)] = new Dictionary<EntityId, TEntity>() { { entity.Id, entity } };
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

    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase
    {
        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            return [];
        }

        return (IEnumerable<TEntity>)dict!.Values;
    }
}