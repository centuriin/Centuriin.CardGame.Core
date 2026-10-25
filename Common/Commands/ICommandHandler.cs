namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandHandler
{
    public Task HandleAsync(ICommand command, CancellationToken token);
}
