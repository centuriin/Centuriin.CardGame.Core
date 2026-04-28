namespace Centuriin.CardGame.Core.Common.Commands;

public interface IConfigurableCommandValidator
{
    public void AddRule<TCommand>(IGameRule rule)
        where TCommand : ICommand;
}