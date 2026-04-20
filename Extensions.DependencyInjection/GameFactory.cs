using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.GameProfiles;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameFactory : IGameSessionFactory
{
    private readonly IGameProfilesRepository _profilesRepository;
    private readonly IGameTypeRepository _gameTypeRepository;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GameFactory(
        IGameProfilesRepository profilesRepository,
        IGameTypeRepository gameTypeRepository,
        IServiceScopeFactory serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesRepository = profilesRepository;

        ArgumentNullException.ThrowIfNull(gameTypeRepository);
        _gameTypeRepository = gameTypeRepository;

        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceScopeFactory = serviceProvider;
    }

    public IGameSession Create(GameSetup setup)
    {
        ArgumentNullException.ThrowIfNull(setup);

        var scope = _serviceScopeFactory.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        var dispatcher = serviceProvider.GetRequiredService<IGamePipelineBuilder>();

        //todo

        var game = ActivatorUtilities.CreateInstance<Game>(serviceProvider, new GameId(Guid.NewGuid()), dispatcher);

        return new GameSession(game, scope);
    }
}
