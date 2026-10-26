namespace Centuriin.CardGame.Core.Common.Commands.Rules;

public interface ICommandRuleFactory
{
    public TRule Create<TRule>() where TRule : ICommandRule;
}