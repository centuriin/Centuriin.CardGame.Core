using Centuriin.CardGame.Core.Common.GameProfiles;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Repositories.InMemory;

public sealed class GameProfilesRepository : IGameProfilesRepository
{
    private readonly IEnumerable<IGameProfile> _gameProfiles;
    private readonly IGameTypeRepository _typeRepository;
    private Dictionary<GameTypeId, IGameProfile>? _gameProfilesMap;


    public GameProfilesRepository(
        IEnumerable<IGameProfile> gameProfiles,
        IGameTypeRepository typeRepository)
    {
        ArgumentNullException.ThrowIfNull(gameProfiles);
        _gameProfiles = gameProfiles;

        ArgumentNullException.ThrowIfNull(typeRepository);
        _typeRepository = typeRepository;
    }

    public async ValueTask<IGameProfile> GetProfileByGameTypIdAsync(GameTypeId typeId, CancellationToken token) =>
        (_gameProfilesMap ??= await GetProfilesMapAsync(_gameProfiles))[typeId];

    private async Task<Dictionary<GameTypeId, IGameProfile>> GetProfilesMapAsync(IEnumerable<IGameProfile> gameProfiles)
    {
        var map = new Dictionary<GameTypeId, IGameProfile>();

        foreach (var profile in gameProfiles)
        {
            var gameType = await _typeRepository.GetGameTypeIdByKey(profile.Key);

            map[gameType] = profile;
        }

        return map;
    }
}