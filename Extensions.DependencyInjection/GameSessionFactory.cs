using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Initialization;
using Centuriin.CardGame.Core.Engine.Loading;
using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameSessionFactory : IGameSessionFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GameSessionFactory(
        IServiceScopeFactory serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceScopeFactory = serviceProvider;
    }

    public async ValueTask<IGameSession> CreateAsync(
        GameSetup setup,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        var scope = _serviceScopeFactory.CreateScope();

        try
        {
            var services = scope.ServiceProvider;

            // todo rework

            var gameId = new GameId(Guid.CreateVersion7());

            var initialStateContext = await services
                .GetRequiredService<IGameContextAccessor<IInitialStateContext>>()
                .GetContextAsync(gameId, token);

            var initializers = services.GetServices<IGameEntityInitializer>();

            foreach (var initializer in initializers)
            {
                await initializer.InitializeAsync(setup, initialStateContext, token);
            }

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
