using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameSession : IGameSession
{
    private readonly IServiceScope _serviceScope;
    private bool _disposed;

    public IGame Game
    {
        get
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return field;
        }
    }

    public GameSession(IGame game, IServiceScope serviceScope)
    {
        ArgumentNullException.ThrowIfNull(game);
        Game = game;

        ArgumentNullException.ThrowIfNull(serviceScope);
        _serviceScope = serviceScope;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _serviceScope.Dispose();

        _disposed = true;
    }
}