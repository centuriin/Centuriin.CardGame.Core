using Centuriin.CardGame.Core.Common.GameProfiles;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameProfilesRepository : IGameProfilesRepository
{
    private readonly IEnumerable<IGameProfile> _gameProfiles;

    public GameProfilesRepository(IEnumerable<IGameProfile> gameProfiles)
    {
        ArgumentNullException.ThrowIfNull(gameProfiles);
        _gameProfiles = gameProfiles;
    }

    public ValueTask<IEnumerable<IGameProfile>> GetGameProfilesAsync(CancellationToken token) =>
        ValueTask.FromResult(_gameProfiles);
}