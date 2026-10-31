using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Root.State;
using Centuriin.CardGame.Core.Common.Storage.Repositories;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameSessionFactory : IGameSessionFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IGameProfilesRepository _profilesRepository;

    public GameSessionFactory(
        IServiceScopeFactory serviceProvider,
        IGameProfilesRepository profilesRepository)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceScopeFactory = serviceProvider;

        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesRepository = profilesRepository;
    }

    public async ValueTask<IGameSession> CreateAsync(
        GameSetup setup,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        var scope = _serviceScopeFactory.CreateScope();

        var profile = await _profilesRepository.GetProfileByGameTypIdAsync(
            setup.GameTypeId,
            token);

        try
        {
            var services = scope.ServiceProvider;

            var configInitializers = services.GetServices<IGameConfigurationInitializer>();

            foreach (var configInitializer in configInitializers)
            {
                configInitializer.Initialize(profile);
            }

            // todo rework
            var state = services.GetRequiredService<IGameState>();

            foreach (var loader in services.GetServices<IGameLoader>())
            {
                await loader.LoadAsync(setup, state, token);
            }

            var game = services.GetRequiredService<IGameFactory>()
                .CreateMaster(setup.GameTypeId);

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
