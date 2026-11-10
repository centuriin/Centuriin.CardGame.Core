namespace Centuriin.CardGame.Core.Engine.Context;

public interface IInitialStateContext : IReadOnlyInitialStateContext
{
    public new IEntitiesCollection Entities { get; }

    IReadOnlyEntitiesCollection IReadOnlyInitialStateContext.Entities => Entities;
}