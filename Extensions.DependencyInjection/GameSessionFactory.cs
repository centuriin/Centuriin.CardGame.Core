using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameSessionFactory : IGameSessionFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GameSessionFactory(IServiceScopeFactory serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceScopeFactory = serviceProvider;
    }

    public async ValueTask<IGameSession> CreateAsync(
        GameSetup setup,
        IGameConfiguration gameConfiguration,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        var scope = _serviceScopeFactory.CreateScope();

        try
        {
            var services = scope.ServiceProvider;

            gameConfiguration.Apply(
                services.GetRequiredService<IGameConfigurationContext>());

            var game = services.GetRequiredService<IGameFactory>()
                .Create<Game>(setup.GameTypeId);

            var executor = services.GetRequiredService<ICommandExecutorFactory>()
                .Create(game);

            foreach (var loader in services.GetServices<IGameLoader>())
            {
                await loader.LoadAsync(setup, game.State, token);
            }

            await game.StartAsync(token);
            return new GameSession(scope, game, executor);
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }
}
