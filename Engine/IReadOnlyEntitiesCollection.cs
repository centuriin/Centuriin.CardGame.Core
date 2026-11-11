using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

public interface IReadOnlyEntitiesCollection
{
    public bool IsEmpty { get; }

    public TEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase;

    public IEntitiesCollection ToCollection();
}
