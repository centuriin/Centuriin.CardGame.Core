using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

public sealed class GameSessionFactory : IGameSessionFactory
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

    public async ValueTask<IGameSession> CreateAsync(GameSetup setup, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        var scope = _serviceScopeFactory.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        var profile = await _profilesRepository.GetProfileByGameTypIdAsync(setup.GameTypeId, token);

        var pipelineConfigurator = 
            serviceProvider.GetRequiredService<IGamePipelineConfigurator>();
        
        profile.Configure(pipelineConfigurator);
        pipelineConfigurator.Setup();

        var commandValidationConfigurator = 
            serviceProvider.GetRequiredService<ICommandValidatationConfigurator>();

        profile.Configure(commandValidationConfigurator);
        commandValidationConfigurator.Setup();

        var game = serviceProvider
            .GetRequiredService<IGameFactory>()
            .Create();

        return new GameSession(game, scope);
    }
}