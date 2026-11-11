using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class ScopedGameContextAccessor<TContext> : IGameContextAccessor<TContext>
    where TContext : IGameContext
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ScopedGameContextAccessor(IServiceScopeFactory scopeFactory)
    {
        ArgumentNullException.ThrowIfNull(scopeFactory);
        _scopeFactory = scopeFactory;
    }

    public async Task<TContext> GetContextAsync(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        using var scope = _scopeFactory.CreateScope();

        var scopedContextAccessor = scope.ServiceProvider
            .GetRequiredKeyedService<IGameContextAccessor<TContext>>("scoped");

        var context = await scopedContextAccessor.GetContextAsync(gameId, token);

        return context;
    }
}
