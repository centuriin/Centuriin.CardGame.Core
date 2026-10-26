using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Storage.Repositories;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameStartupService : IGameStartupService
{
    private readonly IGameSessionFactory _gameSessionFactory;
    private readonly IGameSessionsRepository _sessionsRepository;

    private readonly IGameProfilesRepository _profilesRepository;

    private readonly IGameConfigurationRepository _gameConfigurationRepository;
    private readonly IGameConfigurationFactory _gameConfigurationFactory;

    public GameStartupService(
        IGameSessionFactory gameSessionFactory,
        IGameSessionsRepository sessionsRepository,
        IGameProfilesRepository profilesRepository,
        IGameConfigurationRepository gameConfigurationRepository,
        IGameConfigurationFactory gameConfigurationFactory)
    {
        ArgumentNullException.ThrowIfNull(sessionsRepository);
        _sessionsRepository = sessionsRepository;

        ArgumentNullException.ThrowIfNull(gameSessionFactory);
        _gameSessionFactory = gameSessionFactory;

        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesRepository = profilesRepository;

        ArgumentNullException.ThrowIfNull(gameConfigurationRepository);
        _gameConfigurationRepository = gameConfigurationRepository;

        ArgumentNullException.ThrowIfNull(gameConfigurationFactory);
        _gameConfigurationFactory = gameConfigurationFactory;
    }

    public async Task<IGameSession> StartupGameAsync(GameSetup setup, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);

        token.ThrowIfCancellationRequested();

        var gameConfiguration = await _gameConfigurationRepository.FindAsync(
            setup.GameTypeId,
            token)
            ?? _gameConfigurationFactory.CreateFromProfile(
                await _profilesRepository.GetProfileByGameTypIdAsync(
                    setup.GameTypeId,
                    token));

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
