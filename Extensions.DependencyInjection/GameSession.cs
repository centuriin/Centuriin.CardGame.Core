using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameSession : IGameSession
{
    private bool _disposed;

    private readonly IServiceScope _scope;
    private readonly IGame _game;
    private readonly ICommandExecutor _commands;   

    public GameId Id => _game.Id;
    public GameTypeId GameTypeId => _game.TypeId;

    public GameSession(
        IServiceScope scope,
        IGame game,
        ICommandExecutor commands)
    {
        ArgumentNullException.ThrowIfNull(scope);
        _scope = scope;

        ArgumentNullException.ThrowIfNull(game);
        _game = game;

        ArgumentNullException.ThrowIfNull(commands);
        _commands = commands;
    }

    public async Task<CommandResult> ExecuteAsync(ICommand command, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.GameId != Id)
        {
            throw new InvalidOperationException(
                $"Command game id should be {Id.Value}," +
                $" but found {command.GameId.Value}.");
        }

        return await _commands.ExecuteAsync(command, token);
    }

    public async Task AbortAsync(CancellationToken token) => 
        await _game.AbortAsync(token);

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _scope.Dispose();
        _disposed = true;
    }
}
