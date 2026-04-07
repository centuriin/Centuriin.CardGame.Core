namespace Centuriin.CardGame.Core.Common;

public interface IGameStartupService
{
    public Task<IGame> StartupGameAsync(GameSetup setup, CancellationToken token);
}