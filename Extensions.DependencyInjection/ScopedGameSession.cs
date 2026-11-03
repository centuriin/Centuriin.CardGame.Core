using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Root;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

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

    public async Task<CommandResult> ExecuteAsync(
        ICommand command,
        CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        return await _session.ExecuteAsync(command, token);
    }

    public async Task AbortAsync(CancellationToken token)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        token.ThrowIfCancellationRequested();

        await _session.AbortAsync(token);
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
