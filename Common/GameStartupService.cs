using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Storage.Repositories;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameStartupService : IGameStartupService
{
    private readonly IGameSessionFactory _gameSessionFactory;
    private readonly IGameSessionsRepository _sessionsRepository;

    private readonly IGameProfilesRepository _profilesRepository;

    private readonly IEnumerable<IGameConfigurationInitializer> _configurationInitializers;

    public GameStartupService(
        IGameSessionFactory gameSessionFactory,
        IGameSessionsRepository sessionsRepository,
        IGameProfilesRepository profilesRepository,
        IEnumerable<IGameConfigurationInitializer> gameConfigurationInitializers)
    {
        ArgumentNullException.ThrowIfNull(sessionsRepository);
        _sessionsRepository = sessionsRepository;

        ArgumentNullException.ThrowIfNull(gameSessionFactory);
        _gameSessionFactory = gameSessionFactory;

        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesRepository = profilesRepository;

        ArgumentNullException.ThrowIfNull(gameConfigurationInitializers);
        _configurationInitializers = gameConfigurationInitializers;
    }

    public async Task<IGameSession> StartupGameAsync(GameSetup setup, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        token.ThrowIfCancellationRequested();

        var profile = await _profilesRepository.GetProfileByGameTypIdAsync(
            setup.GameTypeId,
            token);

        foreach (var configIntiailizer in _configurationInitializers)
        {
            configIntiailizer.Initialize(profile);
        }

        var session = await _gameSessionFactory.CreateAsync(
            setup, 
            gameConfiguration,
            token);

        try
        {
            await _sessionsRepository.AddAsync(session, token);
            return session;
        }
        catch
        {
            session.Dispose();
            throw;
        }
    }
}
