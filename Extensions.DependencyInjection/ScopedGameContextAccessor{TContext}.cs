using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Loads a game context through a keyed accessor in a temporary scope.
/// </summary>
/// <typeparam name="TContext">
/// The game context type to load.
/// </typeparam>
internal sealed class ScopedGameContextAccessor<TContext> : IGameContextAccessor<TContext>
    where TContext : IGameContext
{
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// Initializes a new <see cref="ScopedGameContextAccessor{TContext}"/> instance.
    /// </summary>
    /// <param name="scopeFactory">
    /// The factory used to create dependency injection scopes.
    /// </param>
    public ScopedGameContextAccessor(IServiceScopeFactory scopeFactory)
    {
        ArgumentNullException.ThrowIfNull(scopeFactory);
        _scopeFactory = scopeFactory;
    }

    /// <inheritdoc/>
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
