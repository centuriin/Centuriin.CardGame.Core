namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandValidator
{
    public bool IsValid(ICommand command);
}