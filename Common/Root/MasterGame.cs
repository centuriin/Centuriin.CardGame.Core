using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Context;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Root;

public sealed class MasterGame : GameBase
{
    private readonly IGameEventApplier _eventApplier;
    private readonly IGameEventProducer _eventProducer;

    public MasterGame(
        GameId id,
        GameStatus status,
        IGameContextAccessor<IRestorationContext> gameContextAccessor,
        IAsyncDispatcher<IGameApplicationEvent> dispatcher,
        IGameEventApplier eventApplier,
        IGameEventProducer eventProducer)
        : base(id, status, gameContextAccessor, dispatcher)
    {
        ArgumentNullException.ThrowIfNull(eventApplier);
        _eventApplier = eventApplier;

        ArgumentNullException.ThrowIfNull(eventProducer);
        _eventProducer = eventProducer;
    }

    protected override Task<IGameEventsBatch> StartCoreAsync(
        CancellationToken token)
    {
        var appliedEvents = _eventProducer.Produce(new GameStartedEvent(Id));

        return Task.FromResult<IGameEventsBatch>(
            new GameEventsBatch(IncrementVersion(), appliedEvents));
    }

    protected override Task<IGameEventsBatch> ExecuteCoreAsync(
        ICommand command,
        CancellationToken token)
    {
        var @event = command.YieldGameEvent();

        var appliedEvents = _eventProducer.Produce(@event);

        return Task.FromResult<IGameEventsBatch>(
            new GameEventsBatch(IncrementVersion(), appliedEvents));
    }

    protected override Task<IGameEventsBatch> AbortCoreAsync(CancellationToken token)
        => Task.FromResult<IGameEventsBatch>(new GameEventsBatch(IncrementVersion(), []));

    protected override async Task<IGameEventsBatch> RestoreCoreAsync(CancellationToken token)
    {
        var context = await RestorationContextAccessor.GetContextAsync(Id, token);

        foreach (var @event in context.RestorationBatch)
        {
            _eventApplier.Apply(@event);
        }

        return context.RestorationBatch;
    }

    private GameVersion IncrementVersion() => new(Version.Value + 1);
}
