using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Context;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Root;

public sealed class ReplicaGame : GameBase
{
    private readonly IGameEventApplier _eventApplier;
    private readonly IMasterGameBridge _masterGameBridge;

    public ReplicaGame(
        GameDescription description,
        GameStatus status,
        IGameContextAccessor<IRestorationContext> restorationContextAccessor,
        IAsyncDispatcher<IGameApplicationEvent> dispatcher,
        IGameEventApplier eventApplier,
        IMasterGameBridge masterGameBridge)
        : base(description, status, restorationContextAccessor, dispatcher)
    {
        ArgumentNullException.ThrowIfNull(eventApplier);
        _eventApplier = eventApplier;

        ArgumentNullException.ThrowIfNull(masterGameBridge);
        _masterGameBridge = masterGameBridge;
    }

    protected override async Task<IGameEventsBatch> StartCoreAsync(
        CancellationToken token)
    {
        var initialBatch = await _masterGameBridge.StartAsync(token);

        foreach (var @event in initialBatch)
        {
            _eventApplier.Apply(@event);
        }

        return initialBatch;
    }

    protected override async Task<IGameEventsBatch> ExecuteCoreAsync(
        ICommand command,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        var batch = await _masterGameBridge.ExecuteAsync(command, token);

        foreach (var @event in batch)
        {
            _eventApplier.Apply(@event);
        }

        return batch;
    }

    protected override async Task<IGameEventsBatch> AbortCoreAsync(
        CancellationToken token)
        => await _masterGameBridge.AbortAsync(token);

    protected override async Task<IGameEventsBatch> RestoreCoreAsync(CancellationToken token)
    {
        var batch = await _masterGameBridge.RestoreAsync(token);

        foreach (var @event in batch)
        {
            _eventApplier.Apply(@event);
        }

        return batch;
    }
}
