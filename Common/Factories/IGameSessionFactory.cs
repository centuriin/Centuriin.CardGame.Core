using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Factories;

public interface IGameSessionFactory
{
    public ValueTask<IGameSession> CreateAsync(GameSetup setup, CancellationToken token);
}