using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Commands.Rules;

public interface ICommandRule<in TCommand> : ICommandRule
    where TCommand : ICommand
{
    public bool Check(TCommand command, IReadOnlyGameState gameState);

    bool ICommandRule.Check(ICommand command, IReadOnlyGameState gameState) =>
        Check((TCommand)command, gameState);
}
