using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Factories;

public interface IRuleFactory
{
    public TGameRule Create<TGameRule>()
        where TGameRule : IGameRule;
}