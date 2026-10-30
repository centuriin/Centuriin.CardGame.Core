using System.ComponentModel;

using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Context;
using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Root;

public abstract class GameBase : IGame
{
    private GameVersion _version = GameVersion.Default;

    private readonly IAsyncDispatcher<IGameApplicationEvent> _dispatcher;

    public GameId Id { get; }

    public GameTypeId TypeId { get; }

    public GameStatus Status { get; private set; }

    public IReadOnlyGameState State { get; private set; }

    public GameVersion Version
    {
        get => _version;

        private set
        {
            if (value.Value < Version.Value)
            {
                throw new InvalidOperationException(
                    $"New game version {value.Value} can't be less" +
                    $" than current {Version.Value}.");
            }

            _version = value;
        }
    }

    protected IGameEventApplier EventApplier { get; }

    protected GameBase(
        GameId id,
        GameTypeId typeId,
        GameStatus status,
        IReadOnlyGameState state,
        IGameEventApplier eventApplier,
        IAsyncDispatcher<IGameApplicationEvent> dispatcher)
    {
        Id = id;
        TypeId = typeId;

        if (!Enum.IsDefined(status))
        {
            throw new InvalidEnumArgumentException(
                nameof(status),
                (int)status,
                typeof(GameStatus));
        }
        Status = status;

        ArgumentNullException.ThrowIfNull(state);
        State = state;

        ArgumentNullException.ThrowIfNull(eventApplier);
        EventApplier = eventApplier;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;
    }

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (Status is not GameStatus.Created)
        {
            throw new InvalidOperationException(
                $"Game status should be {GameStatus.Created}," +
                $" but current status is {Status}.");
        }

        Status = GameStatus.Running;

        var batch = await StartCoreAsync(token);

        Version = batch.CurrentVersion;

        await _dispatcher.PublishAsync(new GameRunningEvent(Id, batch), token);
    }

    /// <inheritdoc/>
    public async Task ExecuteAsync(ICommand command, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        if (Status is not GameStatus.Running)
        {
            throw new InvalidOperationException(
                $"Game status should be {GameStatus.Running}," +
                $" but current status is {Status}.");
        }

        if (command.GameId != Id)
        {
            throw new InvalidOperationException("" +
                $"Event game id {command.GameId.Value}" +
                $" is not equal to current game id {Id.Value}.");
        }

        var batch = await ApplyCoreAsync(@event, token);

        Version = batch.CurrentVersion;

        await _dispatcher.PublishAsync(
            new NewGameEventsBatchCreatedEvent(Id, batch),
            token);
    }

    /// <inheritdoc/>
    public async Task AbortAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (Status is GameStatus.Aborted)
        {
            return;
        }

        Status = GameStatus.Aborted;

        var batch = await AbortCoreAsync(token);

        Version = batch.CurrentVersion;

        await _dispatcher.PublishAsync(new GameAbortedEvent(Id, Version), token);
    }

    /// <inheritdoc/>
    public async Task RestoreAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var batch = await RestoreCoreAsync(token);

        EventApplier.SilentApply(batch.Events);
        Version = batch.CurrentVersion;

        await _dispatcher.PublishAsync(new GameRestoredEvent(Id, Version), token);
    }

    protected abstract Task<IGameEventsBatch> StartCoreAsync(CancellationToken token);

    protected abstract Task<IGameEventsBatch> ApplyCoreAsync(
        IGameEvent @event,
        CancellationToken token);

    protected abstract Task<IGameEventsBatch> AbortCoreAsync(CancellationToken token);

    protected abstract Task<IGameEventsBatch> RestoreCoreAsync(CancellationToken token);
}
