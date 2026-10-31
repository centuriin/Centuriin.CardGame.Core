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
    private readonly IGameContextAccessor<IRestorationContext> _gameContextAccessor;
    private readonly IGameEventProducer _gameEventProducer;

    public MasterGame(
        GameId id,
        GameTypeId typeId,
        GameStatus status,
        IGameContextAccessor<IRestorationContext> gameContextAccessor,
        IGameEventApplier eventApplier,
        IGameEventProducer gameEventProducer,
        IAsyncDispatcher<IGameApplicationEvent> dispatcher)
        : base(id, typeId, status, eventApplier, dispatcher)
    {
        ArgumentNullException.ThrowIfNull(gameContextAccessor);
        _gameContextAccessor = gameContextAccessor;

        ArgumentNullException.ThrowIfNull(gameEventProducer);
        _gameEventProducer = gameEventProducer;
    }

    protected override Task<IGameEventsBatch> StartCoreAsync(
        CancellationToken token)
    {
        var appliedEvents = _gameEventProducer.Produce(new GameStartedEvent(Id));

        return Task.FromResult<IGameEventsBatch>(
            new GameEventsBatch(IncrementVersion(), appliedEvents));
    }

    protected override Task<IGameEventsBatch> ExecuteCoreAsync(
        ICommand command,
        CancellationToken token)
    {
        var @event = command.YieldGameEvent();

        var appliedEvents = _gameEventProducer.Produce(@event);

        return Task.FromResult<IGameEventsBatch>(
            new GameEventsBatch(IncrementVersion(), appliedEvents));
    }

    protected override Task<IGameEventsBatch> AbortCoreAsync(CancellationToken token)
        => Task.FromResult<IGameEventsBatch>(new GameEventsBatch(IncrementVersion(), []));

    protected override async Task<IGameEventsBatch> RestoreCoreAsync(CancellationToken token)
    {
        var context = await _gameContextAccessor.GetContextAsync(token);

        context.ThrowIfNotApplicable(this);

        return context.RestorationBatch;
    }

    private GameVersion IncrementVersion() => new(Version.Value + 1);
}
