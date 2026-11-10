using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

public interface IGameTypeRepository
{
    public Task<GameTypeId> GetGameTypeIdByKey(string key);
}