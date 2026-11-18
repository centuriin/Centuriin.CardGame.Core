namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

public interface IEntitiesCollection : IReadOnlyEntitiesCollection
{
    public new EditableEntity<TEntity> Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    public void Add(params IEnumerable<EntityBase> entities);

    public void Remove(EntityBase entity);

    public new IEnumerable<EditableEntity<TEntity>> Query<TEntity>()
        where TEntity : EntityBase;
}
