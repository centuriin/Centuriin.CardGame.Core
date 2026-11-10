using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

public interface IEntitiesCollection : IReadOnlyEntitiesCollection
{
    public void AddEntity<TEntity>(TEntity entity)
        where TEntity : EntityBase;

    public new TEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    public new IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase;

    public IReadOnlyEntitiesCollection AsReadOnly();
}
