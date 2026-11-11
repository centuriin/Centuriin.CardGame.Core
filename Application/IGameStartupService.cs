namespace Centuriin.CardGame.Core.Engine;

public interface IGameStartupService
{
    public Task<IGameSession> StartupGameAsync(
        GameDescription setup,
        CancellationToken token);
}