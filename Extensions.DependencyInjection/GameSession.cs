using Centuriin.CardGame.Core.Common.Commands;
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

    public ICommandHandler CommandHandler
    {
        get
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return field;
        }
    }

    public GameSession(
        IServiceScope serviceScope,
        IGame game, 
        ICommandHandler commandHandler)
    {
        ArgumentNullException.ThrowIfNull(serviceScope);
        _serviceScope = serviceScope;

        ArgumentNullException.ThrowIfNull(game);
        Game = game;

        ArgumentNullException.ThrowIfNull(commandHandler);
        CommandHandler = commandHandler;
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