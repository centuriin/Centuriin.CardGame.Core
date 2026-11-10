using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.World;

namespace Centuriin.CardGame.Core.Engine;

public interface IGameStartupService
{
    public Task<IGameSession> StartupGameAsync(GameSetup setup, CancellationToken token);
}