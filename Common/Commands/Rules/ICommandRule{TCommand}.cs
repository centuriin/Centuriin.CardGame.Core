using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands.Rules;

public interface ICommandRule<in TCommand> : ICommandRule
    where TCommand : ICommand
{
    public bool Check(TCommand command, IGameState gameState);

    bool ICommandRule.Check(ICommand command, IGameState gameState) =>
        Check((TCommand)command, gameState);
}
