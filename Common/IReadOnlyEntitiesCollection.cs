using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common;

public interface IReadOnlyEntitiesCollection
{
    public bool IsEmpty { get; }

    public TEntity Get<TEntity>(EntityId id)
        where TEntity : EntityBase;

    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase;
}
