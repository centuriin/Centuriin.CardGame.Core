namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameTypeRepository
{
    public Task<GameTypeId> GetGameTypeIdByKey(string key);
}