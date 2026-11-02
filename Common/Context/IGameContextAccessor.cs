namespace Centuriin.CardGame.Core.Common.Context;

public interface IGameContextAccessor<TContext>
    where TContext : IGameContext
{
    public Task<TContext> GetContextAsync(GameDescription gameDescription, CancellationToken token);
}