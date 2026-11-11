using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

public interface IGameStartupService
{
    public Task<IGameSession> StartupGameAsync(
        GameDescription setup,
        CancellationToken token);
}