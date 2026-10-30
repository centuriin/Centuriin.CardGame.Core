namespace Centuriin.CardGame.Core.Common.Commands.Rules;

using Centuriin.CardGame.Core.Common.Root.State;

public interface ICommandRule
{
    public bool Check(ICommand command, IGameState gameState);
}
