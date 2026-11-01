using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Root;

public interface IGameFactory
{
    public IGame CreateMaster(GameTypeId typeId);

    public IGame CreateReplica(IGameSnapshot snapshot);
}