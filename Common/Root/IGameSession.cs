using Centuriin.CardGame.Core.Common.Commands;

namespace Centuriin.CardGame.Core.Common.Root;

public interface IGameSession : IDisposable
{
    public GameId Id { get; }

    public GameTypeId GameTypeId { get; }

    public Task StartAsync(CancellationToken token);

    public Task<CommandResult> ExecuteAsync(ICommand command, CancellationToken token);

    public Task AbortAsync(CancellationToken token);
}
