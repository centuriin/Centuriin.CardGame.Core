using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.GameProfiles;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameFactory : IGameSessionFactory
{
    private readonly IGameProfilesRepository _profilesRepository;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GameFactory(
        IGameProfilesRepository profilesRepository,
        IServiceScopeFactory serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesRepository = profilesRepository;

        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceScopeFactory = serviceProvider;
    }

    public async ValueTask<IGameSession> CreateAsync(GameSetup setup, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        var scope = _serviceScopeFactory.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        var pipelineBuilder = serviceProvider.GetRequiredService<IGamePipelineBuilder>();

        var profile = await _profilesRepository.GetProfileByGameTypIdAsync(setup.GameTypeId, token);

        profile.Configure(pipelineBuilder);

        pipelineBuilder.Build();

        var game = ActivatorUtilities.CreateInstance<Game>(serviceProvider, new GameId(Guid.NewGuid()));

        return new GameSession(game, scope);
    }
}