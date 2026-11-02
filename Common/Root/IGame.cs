using Centuriin.CardGame.Core.Common.Commands;

namespace Centuriin.CardGame.Core.Common.Root;

public interface IGame
{
    public GameDescription Description { get; }

    public GameStatus Status { get; }

    public Task StartAsync(CancellationToken token);

    public Task ExecuteAsync(ICommand command, CancellationToken token);

    public Task AbortAsync(CancellationToken token);

    public Task RestoreAsync(CancellationToken token);
}