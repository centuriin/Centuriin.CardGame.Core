using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IGameTypeRepository
{
    public Task<GameTypeId> GetGameTypeIdByKey(string key);
}