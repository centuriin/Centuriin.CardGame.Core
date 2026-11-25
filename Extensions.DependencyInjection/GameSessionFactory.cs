using Centuriin.CardGame.Core.Application;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Loading;
using Centuriin.CardGame.Core.Engine.Root;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Loads and creates an authoritative game in a dedicated dependency injection scope.
/// </summary>
internal sealed class GameSessionFactory : IGameSessionFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Initializes a new <see cref="GameSessionFactory"/> instance.
    /// </summary>
    /// <param name="serviceScopeFactory">
    /// The factory used to create a dedicated scope for each session.
    /// </param>
    public GameSessionFactory(
        IServiceScopeFactory serviceScopeFactory)
    {
        ArgumentNullException.ThrowIfNull(serviceScopeFactory);
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <inheritdoc/>
    public async ValueTask<IGameSession> CreateAsync(
        GameDescription description,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(description);

        var scope = _serviceScopeFactory.CreateScope();

        try
        {
            var services = scope.ServiceProvider;

            // todo rework

            var gameId = description.GameId;

            var gameLoaders = services.GetServices<IGameLoader>();

            foreach (var loader in gameLoaders)
            {
                await loader.LoadAsync(gameId, token);
            }

            var game = services.GetRequiredService<IGameFactory>()
                .CreateMaster(gameId);

            var session = new ScopedGameSession(
                scope,
                ActivatorUtilities.CreateInstance<GameSession>(
                    services,
                    description.GameTypeId,
                    game));

            return session;
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }
}
