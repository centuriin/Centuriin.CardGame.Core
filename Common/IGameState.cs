using Centuriin.CardGame.Core.Common.Entities;

namespace Centuriin.CardGame.Core.Common;

public interface IGameState
{
    public void AddEntity<TEntity>(TEntity entity)
        where TEntity : EntityBase;
}
