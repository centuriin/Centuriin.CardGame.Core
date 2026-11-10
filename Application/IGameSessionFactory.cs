using Centuriin.CardGame.Core.Engine;

namespace Application;

public interface IGameSessionFactory
{
    public ValueTask<IGameSession> CreateAsync(
        GameSetup setup,
        CancellationToken token);
}