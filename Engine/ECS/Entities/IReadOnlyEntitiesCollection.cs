namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

public interface IReadOnlyEntitiesCollection : IEnumerable<EntityBase>
{
    public bool IsEmpty { get; }

    public TEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase;
}
