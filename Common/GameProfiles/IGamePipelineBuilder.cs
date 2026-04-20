namespace Centuriin.CardGame.Core.Common.GameProfiles;

public interface IGamePipelineBuilder : IConfigurableGamePipelineBuilder
{
    public void Build();
}