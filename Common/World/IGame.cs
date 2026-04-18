using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGame
{
    public IGameState State { get; }

    public Task ApplyAsync(IGameEvent @event, CancellationToken token);
}