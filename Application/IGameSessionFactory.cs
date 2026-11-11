using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

public interface IGameSessionFactory
{
    public ValueTask<IGameSession> CreateAsync(
        GameDescription setup,
        CancellationToken token);
}