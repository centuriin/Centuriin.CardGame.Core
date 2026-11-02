using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common.Context;

public interface IInitialStateContext : IGameContext
{
    public IReadOnlyCollection<TEntity> GetEntities<TEntity>()
        where TEntity : EntityBase;
}