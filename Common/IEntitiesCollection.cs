using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common;

public interface IEntitiesCollection : IReadOnlyEntitiesCollection
{
    public void AddEntity<TEntity>(TEntity entity)
        where TEntity : EntityBase;

    public new TEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase;
    TEntity IReadOnlyEntitiesCollection.Get<TEntity>(EntityId id) =>
        (TEntity)Get<TEntity>(id).Copy();

    public new IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase;

    IEnumerable<TEntity> IReadOnlyEntitiesCollection.Query<TEntity>() =>
        Query<TEntity>().Select(x => (TEntity)x.Copy());

    public IReadOnlyEntitiesCollection AsReadOnly();
}
