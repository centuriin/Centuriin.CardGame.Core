namespace Centuriin.CardGame.Core.Common.Commands;

public interface IGameRulesBuilder : IConfigurableGameRulesBuilder
{
    public ICommandValidator Build();
}