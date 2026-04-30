namespace Centuriin.CardGame.Core.Common.World;

public interface IGameFactory
{
    public TGame Create<TGame>()
        where TGame : IGame;

    public TGame Create<TGame>(IGameSnapshot snapshot)
        where TGame : IGame;
}