using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common;

public interface IGameStartupService
{
    public Task<IGameSession> StartupGameAsync(GameSetup setup, CancellationToken token);
}