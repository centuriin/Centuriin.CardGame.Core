namespace Centuriin.CardGame.Core.Common.Context;

public interface IReadOnlyInitialStateContext : IGameContext
{
    public IReadOnlyEntitiesCollection Entities { get; }
}
