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

    public GameId Id => _session.Id;

    public GameTypeId GameTypeId => _session.GameTypeId;

    public ScopedGameSession(IServiceScope scope, GameSession session)
    {
        ArgumentNullException.ThrowIfNull(_scope);
        _scope = scope;

        ArgumentNullException.ThrowIfNull(session);
        _session = session;
    }

    public Task StartAsync(CancellationToken token) => _session.StartAsync(token);

    public Task<CommandResult> ExecuteAsync(
        ICommand command,
        CancellationToken token)
        => _session.ExecuteAsync(command, token);

    public Task AbortAsync(CancellationToken token) => _session.AbortAsync(token);

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _session.Dispose();
        _scope.Dispose();
    }
}
