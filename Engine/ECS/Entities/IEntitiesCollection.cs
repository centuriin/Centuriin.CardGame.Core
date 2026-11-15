namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

public interface IEntitiesCollection : IReadOnlyEntitiesCollection
{
    public void Add<TEntity>(TEntity entity)
        where TEntity : EntityBase;

    public void Replace<TEntity>(TEntity replacement)
        where TEntity : EntityBase;
}
