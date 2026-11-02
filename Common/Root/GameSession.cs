using System.Diagnostics;

using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Observability;

namespace Centuriin.CardGame.Core.Common.Root;

// Todo: maybe need to create other DI collection for game 
public sealed class GameSession : IGameSession
{
    private bool _isDisposed;

    private readonly Activity? _sessionActivity;
    private readonly ActivityContext _activityContext;

    private readonly IGame _game;
    private readonly ICommandValidator _validator;

    public GameId GameId => _game.Id;

    public GameSession(
        IGame game,
        ICommandValidator validator)
    {
        ArgumentNullException.ThrowIfNull(game);
        _game = game;

        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;

        _sessionActivity = Telemetry.StartGameSessionActivity(this);
        _activityContext = _sessionActivity?.Context ?? default;
    }

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        using var _ = Telemetry.StartGameActivity(_game, _activityContext);

        await _game.StartAsync(token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> ExecuteAsync(ICommand command, CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        using var _ = Telemetry.StartCommandActivity(command, _activityContext);

        if (_validator.IsValid(command))
        {
            await _game.ExecuteAsync(command, token);

            return new(true, _game.Version);
        }

        return new(false, _game.Version);
    }

    /// <inheritdoc/>
    public async Task AbortAsync(CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        using var _ = Telemetry.StartAbortGameActivity(_game, _activityContext);

        await _game.AbortAsync(token);
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
