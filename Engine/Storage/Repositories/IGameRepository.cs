using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Storage.Repositories;

public interface IGameRepository
{
    public Task UpdateVersion(GameId gameId, GameVersion newVersion);
}
