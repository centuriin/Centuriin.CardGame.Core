using Centuriin.CardGame.Core.Common.Commands;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGame
{
    public GameId GameId { get; }

    public GameStatus Status { get; }

    public IGameState State { get; }

    public Task<ICommandResult> ExecuteAsync(ICommand command, CancellationToken token);

    public Task StartAsync(CancellationToken token);
}