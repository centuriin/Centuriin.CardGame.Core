using Centuriin.CardGame.Core.Common.GameProfiles;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IGameProfilesRepository
{
    public ValueTask<IGameProfile> GetProfileByGameTypIdAsync(GameTypeId typeId, CancellationToken token);
}