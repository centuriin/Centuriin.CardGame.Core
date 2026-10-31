using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Root;

public sealed class ReplicaGame : GameBase
{
    private readonly IMasterGameBridge _masterGameBridge;

    public ReplicaGame(
        GameId id,
        GameTypeId typeId,
        GameStatus status,
        IGameEventApplier eventApplier,
        IAsyncDispatcher<IGameApplicationEvent> dispatcher,
        IMasterGameBridge masterGameBridge)
        : base(id, typeId, status, eventApplier, dispatcher)
    {
        ArgumentNullException.ThrowIfNull(masterGameBridge);
        _masterGameBridge = masterGameBridge;
    }

    protected override async Task<IGameEventsBatch> StartCoreAsync(
        CancellationToken token)
        => await _masterGameBridge.StartAsync(token);

    protected override async Task<IGameEventsBatch> ExecuteCoreAsync(
        ICommand command,
        CancellationToken token)
        => await _masterGameBridge.ExecuteAsync(command, token);

    protected override async Task<IGameEventsBatch> AbortCoreAsync(
        CancellationToken token)
        => await _masterGameBridge.AbortAsync(token);

    protected override async Task<IGameEventsBatch> RestoreCoreAsync(CancellationToken token)
        => await _masterGameBridge.RestoreAsync(token);
}
