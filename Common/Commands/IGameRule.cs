using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface IGameRule
{
    public RuleResult Check(IGameState gameState, ICommand command);
}
