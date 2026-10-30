using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common.Root.State;

public interface IGameState : IReadOnlyGameState
{
    public ITurnAutomat TurnAutomat { get; }

    public void AddEntity<TEntity>(TEntity entity)
        where TEntity : EntityBase;

    public new TEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    public new IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase;
}
