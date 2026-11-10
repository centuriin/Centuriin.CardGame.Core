namespace Centuriin.CardGame.Core.Engine.Commands.Rules;

public interface ICommandRuleFactory
{
    public TRule Create<TRule>() where TRule : ICommandRule;
}