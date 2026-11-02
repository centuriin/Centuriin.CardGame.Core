using Centuriin.CardGame.Core.Common.Loading.Configuration;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameProfilesRepository
{
    public ValueTask<IGameProfile> GetProfileByGameTypIdAsync(GameTypeId typeId, CancellationToken token);
}