namespace Centuriin.CardGame.Core.Common.Commands.Rules;

public interface IRuleFactory
{
    public TGameRule Create<TGameRule>()
        where TGameRule : IGameRule;
}