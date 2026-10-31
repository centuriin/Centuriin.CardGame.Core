using Centuriin.CardGame.Core.Common.Root;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGameSessionFactory
{
    public ValueTask<IGameSession> CreateAsync(
        GameSetup setup,
        CancellationToken token);
}