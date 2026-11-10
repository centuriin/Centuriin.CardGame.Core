using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Engine.Storage.Repositories;

public interface IGameProfilesRepository
{
    public ValueTask<IGameProfile> GetProfileByGameTypIdAsync(GameTypeId typeId, CancellationToken token);
}