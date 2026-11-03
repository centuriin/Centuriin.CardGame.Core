namespace Centuriin.CardGame.Core.Common.Context;

public interface IInitialStateContext : IReadOnlyInitialStateContext
{
    public new IEntitiesCollection Entities { get; }
}