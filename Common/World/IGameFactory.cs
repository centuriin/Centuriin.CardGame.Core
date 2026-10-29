using Centuriin.CardGame.Core.Common.Root;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGameFactory
{
    public TGame Create<TGame>(GameTypeId typeId)
        where TGame : IGame;

    public TGame Create<TGame>(IGameSnapshot snapshot)
        where TGame : IGame;
}