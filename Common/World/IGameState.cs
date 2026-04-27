using Centuriin.CardGame.Core.Common.Entities;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGameState
{
    public ITurnAutomat TurnAutomat { get; }

    public void AddEntity<TEntity>(TEntity entity)
        where TEntity : EntityBase;

    public TEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase;
}
