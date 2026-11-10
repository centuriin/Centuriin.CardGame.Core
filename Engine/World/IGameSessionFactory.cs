using Centuriin.CardGame.Core.Engine.Root;

namespace Centuriin.CardGame.Core.Engine.World;

public interface IGameSessionFactory
{
    public ValueTask<IGameSession> CreateAsync(
        GameSetup setup,
        CancellationToken token);
}