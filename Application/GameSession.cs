using System.Diagnostics;

using Centuriin.CardGame.Core.Application.Events;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Root;

namespace Centuriin.CardGame.Core.Application;

public sealed class GameSession : IGameSession
{
    private bool _isDisposed;

    private readonly Activity? _sessionActivity;
    private readonly ActivityContext _activityContext;

    private readonly IGame _game;
    private readonly IGameContextAccessor<IRestorationContext> _restorationContextAccessor;
    private readonly IAsyncDispatcher<IGameApplicationEvent> _dispatcher;

    public GameId GameId => _game.Id;

    public GameSession(
        IGame game,
        IGameContextAccessor<IRestorationContext> restorationContextAccessor,
        IAsyncDispatcher<IGameApplicationEvent> dispatcher)
    {
        ArgumentNullException.ThrowIfNull(game);
        _game = game;

        ArgumentNullException.ThrowIfNull(restorationContextAccessor);
        _restorationContextAccessor = restorationContextAccessor;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;

        _sessionActivity = Telemetry.StartGameSessionActivity(this);
        _activityContext = _sessionActivity?.Context ?? default;
    }

    public async Task StartAsync(CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        token.ThrowIfCancellationRequested();

        using var _ = Telemetry.StartGameActivity(_game.Id, _activityContext);

        var engineResult = _game.Start();

        IGameApplicationEvent applicationEvent = engineResult switch
        {
            ExecutedResult executed =>
                new GameRunningEvent(_game.Id, executed.Batch),

            PreparedResult prepared =>
                new GamePreparedEvent(_game.Id, prepared.GameVersion),

            _ => throw new InvalidOperationException(
                $"Engine result type {engineResult.GetType().Name} can't be handled.")
        };

        await _dispatcher.PublishAsync(applicationEvent, token);
    }


    /// <inheritdoc/>
    public async Task ExecuteAsync(ICommand command, CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        using var _ = Telemetry.StartCommandActivity(command, _activityContext);

        var engineResult = _game.Execute(command);

        IGameApplicationEvent applicationEvent = engineResult switch
        {
            RejectedResult rejected =>
                new GameCommandDeclinedEvent(_game.Id, rejected.GameVersion, command),

            ExecutedResult executed =>
                new NewGameEventsBatchCreatedEvent(_game.Id, executed.Batch),

            PreparedResult prepared =>
                new GameCommandPreparedEvent(_game.Id, prepared.GameVersion, command),

            _ => throw new InvalidOperationException(
                $"Engine result type {engineResult.GetType().Name} can't be handled.")
        };

        await _dispatcher.PublishAsync(applicationEvent, token);
    }

    public async Task RestoreAsync(CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        token.ThrowIfCancellationRequested();

        var context = await _restorationContextAccessor.GetContextAsync(_game.Id, token);

        _game.Apply(context.RestorationBatch);

        await _dispatcher.PublishAsync(
            new GameRestoredEvent(_game.Id, _game.Version),
            token);
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _sessionActivity?.Dispose();

        _isDisposed = true;
    }
}
