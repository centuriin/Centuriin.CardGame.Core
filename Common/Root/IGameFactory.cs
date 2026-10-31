using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGameFactory
{
    public IGame CreateMaster(GameTypeId typeId);

    public IGame CreateReplica(IGameSnapshot snapshot);
}