using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Root;

public interface IMasterGameBridge
{
    public Task<IGameEventsBatch> ConnectAsync(CancellationToken token);

    public Task<IGameEventsBatch> SendAsync(IGameEvent @event, CancellationToken token);

    public Task<IGameEventsBatch> DisconnectAsync(CancellationToken token);
}
