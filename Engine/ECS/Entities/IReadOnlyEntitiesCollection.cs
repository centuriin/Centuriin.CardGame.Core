namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

public interface IReadOnlyEntitiesCollection
{
    public bool IsEmpty { get; }

    public IReadOnlyEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    public IEnumerable<IReadOnlyEntity> Query<TEntity>()
        where TEntity : EntityBase;

    public IEntitiesCollection ToCollection();
}
