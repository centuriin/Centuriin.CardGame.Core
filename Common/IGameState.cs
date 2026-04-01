using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.Centuriin.Core.Common;

namespace Centuriin.CardGame.Core.Common;

public interface IGameState
{
    public GameId GameId { get; }

    public void AddEntity<TEntity, TId>(TEntity entity)
        where TEntity : EntityBase<TId>
        where TId : struct, IEquatable<TId>;

    public TEntity Get<TEntity, TId>(TId id)
        where TEntity : EntityBase<TId>
        where TId : struct, IEquatable<TId>;

    public IEnumerable<TEntity> Query<TEntity>()
        where TEntity : EntityBase;
}
