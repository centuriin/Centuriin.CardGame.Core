using Application;

namespace Centuriin.CardGame.Core.Engine;

public interface IGameStartupService
{
    public Task<IGameSession> StartupGameAsync(
        GameSetup setup,
        CancellationToken token);
}