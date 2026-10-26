namespace Centuriin.CardGame.Core.Common.Configuration;

public interface IGameConfiguration
{
    public void Apply(IGameConfigurationContext context);
}