using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Root;

public interface IMasterGameBridge
{
    public Task<IGameEventsBatch> StartAsync(CancellationToken token);

    public Task<IGameEventsBatch> ExecuteAsync(ICommand command, CancellationToken token);

    public Task<IGameEventsBatch> AbortAsync(CancellationToken token);

    public Task<IGameEventsBatch> RestoreAsync(CancellationToken token);
}
