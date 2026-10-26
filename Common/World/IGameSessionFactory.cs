using Centuriin.CardGame.Core.Common.Configuration;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGameSessionFactory
{
    public ValueTask<IGameSession> CreateAsync(
        GameSetup setup,
        IGameConfiguration gameConfiguration,
        CancellationToken token);
}