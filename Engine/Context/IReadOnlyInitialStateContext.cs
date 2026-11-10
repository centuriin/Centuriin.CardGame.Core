namespace Centuriin.CardGame.Core.Engine.Context;

public interface IReadOnlyInitialStateContext : IGameContext
{
    public IReadOnlyEntitiesCollection Entities { get; }
}
