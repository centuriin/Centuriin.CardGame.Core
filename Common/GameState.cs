using System.Collections;

using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.Centuriin.Core.Common;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameState : IGameState
{
    private readonly Dictionary<Type, IDictionary> _entities = [];

    public GameId GameId { get; }

    public GameState(GameId gameId)
    {
        GameId = gameId;
    }

    public void AddEntity<TEntity, TId>(TEntity entity)
        where TEntity : EntityBase<TId>
        where TId : struct, IEquatable<TId>
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!_entities.TryGetValue(typeof(TEntity), out var dict))
        {
            _entities[typeof(TEntity)] = new Dictionary<TId, TEntity>() { { entity.Id, entity } };
            return;
        }

        ((Dictionary<TId, TEntity>)dict)[entity.Id] = entity;
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