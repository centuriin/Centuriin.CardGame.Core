using Centuriin.CardGame.Core.Engine.Commands;

namespace Centuriin.CardGame.Core.Engine.Root;

public interface IGameSession : IDisposable
{
    public GameId GameId { get; }

    public Task StartAsync(CancellationToken token);

    public Task ExecuteAsync(ICommand command, CancellationToken token);

    public Task RestoreAsync(CancellationToken token);
}