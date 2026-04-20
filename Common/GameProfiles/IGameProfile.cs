namespace Centuriin.CardGame.Core.Common.GameProfiles;

public interface IGameProfile
{
    public string Key { get; }

    public void Configure(IConfigurableGamePipelineBuilder builder);
}