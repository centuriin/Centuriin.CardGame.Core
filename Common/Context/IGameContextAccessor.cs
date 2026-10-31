namespace Centuriin.CardGame.Core.Common.Context;

public interface IGameContextAccessor<TContext>
    where TContext : IGameContext
{
    public Task<TContext> GetContextAsync(CancellationToken token);
}