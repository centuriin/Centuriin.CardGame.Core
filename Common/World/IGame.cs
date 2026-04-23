using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGame
{
    public GameId GameId { get; }

    public IGameState State { get; }

    public Task<ICommandResult> ExecuteAsync(ICommand command, CancellationToken token);

    internal Task ApplyAsync(IPrimaryEvent @event, CancellationToken token);
}