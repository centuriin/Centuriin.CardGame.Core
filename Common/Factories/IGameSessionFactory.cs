using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Factories;

public interface IGameSessionFactory
{
    public IGameSession Create(GameSetup setup);
}