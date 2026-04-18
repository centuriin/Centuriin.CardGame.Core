using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Factories;

public interface IGameFactory
{
    public IGame Create(GameSetup setup);
}