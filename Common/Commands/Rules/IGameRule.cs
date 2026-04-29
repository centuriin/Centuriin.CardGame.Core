using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands.Rules;

public interface IGameRule
{
    public RuleResult Check(IGameState gameState, ICommand command);
}
