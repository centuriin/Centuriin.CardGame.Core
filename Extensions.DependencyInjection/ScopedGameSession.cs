using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Root;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

// Todo: maybe need to create other DI collection for game 
internal sealed class ScopedGameSession : IGameSession
{
    private bool _isDisposed;

    private readonly IServiceScope _scope;
    private readonly GameSession _session;

    public GameId GameId => _session.GameId;

    public ScopedGameSession(IServiceScope scope, GameSession session)
    {
        ArgumentNullException.ThrowIfNull(scope);
        _scope = scope;

        ArgumentNullException.ThrowIfNull(session);
        _session = session;
    }

    public async Task StartAsync(CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        token.ThrowIfCancellationRequested();

        await _session.StartAsync(token);
    }

    public async Task ExecuteAsync(ICommand command, CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        await _session.ExecuteAsync(command, token);
    }

    public async Task RestoreAsync(CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        token.ThrowIfCancellationRequested();

        await _session.RestoreAsync(token);
    }

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
