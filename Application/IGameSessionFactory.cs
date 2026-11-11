using Centuriin.CardGame.Core.Engine;

namespace Application;

public interface IGameSessionFactory
{
    public ValueTask<IGameSession> CreateAsync(
        GameDescription setup,
        CancellationToken token);
}