namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandExecutor
{
    public Task<CommandResult> ExecuteAsync(ICommand command, CancellationToken token);
}
