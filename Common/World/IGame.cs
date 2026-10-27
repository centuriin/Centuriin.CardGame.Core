using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGame
{
    public GameId Id { get; }

    public GameTypeId TypeId { get; }

    public GameStatus Status { get; }

    public IGameState State { get; }

    public GameVersion Version { get; }

    internal Task StartAsync(CancellationToken token);

    internal Task ApplyAsync(IGameEvent @event, CancellationToken token);

    internal Task AbortAsync(CancellationToken token);
}