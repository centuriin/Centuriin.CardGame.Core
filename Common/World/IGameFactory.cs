using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGameFactory
{
    public TGame Create<TGame>(GameTypeId typeId)
        where TGame : IGame;

    public TGame Create<TGame>(IGameSnapshot snapshot)
        where TGame : IGame;
}