namespace Centuriin.CardGame.Core.Engine.Context;

public interface IGameContextAccessor<TContext>
    where TContext : IGameContext
{
    public Task<TContext> GetContextAsync(GameId gameId, CancellationToken token);
}