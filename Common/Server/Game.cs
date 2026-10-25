using System.ComponentModel;

using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Server;

public sealed class Game : IGame
{
    private readonly IGameEventApplier _eventApplier;
    private readonly IAsyncDispatcher<IApplicationEvent> _dispatcher;

    public GameId Id { get; }

    public GameStatus Status { get; private set; }

    public IGameState State { get; }

    public GameVersion Version { get; private set; } = GameVersion.Default;

    public Game(
        GameId id,
        GameStatus gameStatus,
        IGameState gameState,
        IGameEventApplier eventApplier,
        IAsyncDispatcher<IApplicationEvent> dispatcher,
        GameVersion version) :
        this(id, gameStatus, gameState, eventApplier, dispatcher)
    {
        Version = version;
    }

    public Game(
        GameId id,
        GameStatus gameStatus,
        IGameState gameState,
        IGameEventApplier eventApplier,
        IAsyncDispatcher<IApplicationEvent> dispatcher)
    {
        Id = id;

        if (!Enum.IsDefined(gameStatus))
        {
            throw new InvalidEnumArgumentException(
                nameof(gameStatus),
                (int)gameStatus,
                typeof(GameStatus));
        }
        Status = gameStatus;

        ArgumentNullException.ThrowIfNull(gameState);
        State = gameState;

        ArgumentNullException.ThrowIfNull(eventApplier);
        _eventApplier = eventApplier;

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
        Version++;

        var batch = _eventApplier.Apply(new GameStartedEvent(Id));

        await _dispatcher.PublishAsync(new GameRunningEvent(Id, Version), token);

        await _dispatcher.PublishAsync(
            new NewGameEventsBatchCreatedEvent(Id, Version, batch),
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
        Version++;

        await _dispatcher.PublishAsync(new GameAbortedEvent(Id, Version), token);
    }

    /// <inheritdoc/>
    async Task IGame.ApplyAsync(IGameEvent @event, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        token.ThrowIfCancellationRequested();

        if (Status is not GameStatus.Running)
        {
            throw new InvalidOperationException(
                $"Game status should be {GameStatus.Running}," +
                $" but current status is {Status}.");
        }

        if (@event.GameId != Id)
        {
            throw new InvalidOperationException("" +
                $"Event game id {@event.GameId.Value}" +
                $" is not equal to current game id {Id.Value}.");
        }

        Version++;

        var batch = _eventApplier.Apply(@event);

        await _dispatcher.PublishAsync(
            new NewGameEventsBatchCreatedEvent(Id, Version, batch),
            token);
    }
}