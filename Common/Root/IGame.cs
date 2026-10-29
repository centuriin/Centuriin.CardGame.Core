using Centuriin.CardGame.Core.Common.Context;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Root;

public interface IGame
{
    public GameId Id { get; }

    public GameTypeId TypeId { get; }

    public GameStatus Status { get; }

    public IGameState State { get; }

    public GameVersion Version { get; }

    public Task StartAsync(CancellationToken token);

    public Task ApplyAsync(IGameEvent @event, CancellationToken token);

    public Task AbortAsync(CancellationToken token);

    public Task RestoreAsync(IRestorationContext restorationContext, CancellationToken token);
}