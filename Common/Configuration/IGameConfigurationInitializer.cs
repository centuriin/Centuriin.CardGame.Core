namespace Centuriin.CardGame.Core.Common.Configuration;

public interface IGameConfigurationInitializer
{
    public void Initialize(IGameProfile profile);
}