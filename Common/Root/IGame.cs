using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Root;

public interface IGame
{
    public GameId Id { get; }

    public GameTypeId TypeId { get; }

    public GameStatus Status { get; }

    public GameVersion Version { get; }

    public Task StartAsync(CancellationToken token);

    public Task ExecuteAsync(ICommand command, CancellationToken token);

    public Task AbortAsync(CancellationToken token);

    public Task RestoreAsync(CancellationToken token);
}