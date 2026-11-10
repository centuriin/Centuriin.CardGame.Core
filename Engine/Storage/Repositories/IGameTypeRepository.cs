namespace Centuriin.CardGame.Core.Engine.Storage.Repositories;

public interface IGameTypeRepository
{
    public Task<GameTypeId> GetGameTypeIdByKey(string key);
}