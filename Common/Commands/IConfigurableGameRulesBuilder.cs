namespace Centuriin.CardGame.Core.Common.Commands;

public interface IConfigurableGameRulesBuilder
{
    public IConfigurableGameRulesBuilder AddRule<TCommand, TRule>()
        where TCommand : ICommand
        where TRule : IGameRule;
}