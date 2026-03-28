using Centuriin.CardGame.Core.Common.Entities;

namespace Centuriin.CardGame.Core.Common;

public interface IGameState
{
    public void AddEntity<TEntity, TId>(TEntity entity)
        where TEntity : EntityBase<TId>
        where TId : struct, IEquatable<TId>;

    public IEnumerable<TEntity> Query<TEntity, TId>()
        where TEntity : EntityBase<TId>
        where TId : struct, IEquatable<TId>;
}
