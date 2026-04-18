using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGame
{
    public GameId GameId { get; }

    public IGameState State { get; }

    public Task ApplyAsync(IGameEvent @event, CancellationToken token);
}