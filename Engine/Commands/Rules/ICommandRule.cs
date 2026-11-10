namespace Centuriin.CardGame.Core.Engine.Commands.Rules;

using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Root.State;

public interface ICommandRule
{
    public bool Check(ICommand command, IReadOnlyGameState gameState);
}
