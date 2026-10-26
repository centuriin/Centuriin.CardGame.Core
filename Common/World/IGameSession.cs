using Centuriin.CardGame.Core.Common.Commands;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGameSession : IDisposable
{
    public GameId Id { get; }

    public GameTypeId GameTypeId { get; }

    public Task<CommandResult> ExecuteAsync(ICommand command, CancellationToken token);

    public Task AbortAsync(CancellationToken token);
}
