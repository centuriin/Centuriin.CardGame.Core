using Centuriin.CardGame.Core.Application;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

// Todo: maybe need to create other DI collection for game 
/// <summary>
/// Owns a game session and the dependency injection scope used by that session.
/// </summary>
internal sealed class ScopedGameSession : IGameSession
{
    private bool _isDisposed;

    private readonly IServiceScope _scope;
    private readonly GameSession _session;

    /// <inheritdoc/>
    public GameId GameId => _session.GameId;

    /// <inheritdoc/>
    public GameTypeId GameTypeId => _session.GameTypeId;

    /// <summary>
    /// Initializes a new <see cref="ScopedGameSession"/> instance.
    /// </summary>
    /// <param name="scope">
    /// The dependency injection scope owned by the session.
    /// </param>
    /// <param name="session">
    /// The game session associated with the operation.
    /// </param>
    public ScopedGameSession(IServiceScope scope, GameSession session)
    {
        ArgumentNullException.ThrowIfNull(scope);
        _scope = scope;

        ArgumentNullException.ThrowIfNull(session);
        _session = session;
    }

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        token.ThrowIfCancellationRequested();

        await _session.StartAsync(token);
    }

    /// <inheritdoc/>
    public async Task ExecuteAsync(ICommand command, CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        await _session.ExecuteAsync(command, token);
    }

    /// <inheritdoc/>
    public async Task RestoreAsync(CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        token.ThrowIfCancellationRequested();

        await _session.RestoreAsync(token);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _session.Dispose();
        _scope.Dispose();

        _isDisposed = true;
    }
}
