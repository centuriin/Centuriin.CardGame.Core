using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;
using Centuriin.CardGame.Core.Common.Server;
using Centuriin.CardGame.Core.Common.Storage.Repositories;
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

        var profile = await _profilesRepository.GetProfileByGameTypIdAsync(
            setup.GameTypeId, 
            token);

        var pipelineConfigurator =
            serviceProvider.GetRequiredService<IGamePipelineConfigurator>();

        profile.Configure(pipelineConfigurator);
        pipelineConfigurator.Configure();

        var commandValidatorConfigurator =
            serviceProvider.GetRequiredService<ICommandValidatorConfigurator>();

        profile.Configure(commandValidatorConfigurator);
        commandValidatorConfigurator.Configure();

        var game = serviceProvider
            .GetRequiredService<IGameFactory>()
            .Create<Game>();

        var commandHandler = serviceProvider
            .GetRequiredService<ICommandHandlerFactory>()
            .Create(game);

        return new GameSession(scope, game, commandHandler);
    }
}