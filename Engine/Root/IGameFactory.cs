using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

public interface IGameFactory
{
    public IGame CreateMaster(GameId gameId);

    public IGame CreateReplica(IGameSnapshot snapshot);
}