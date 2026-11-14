using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Context;

public interface IReadOnlyInitialStateContext : IGameContext
{
    public IReadOnlyEntitiesCollection Entities { get; }
}
