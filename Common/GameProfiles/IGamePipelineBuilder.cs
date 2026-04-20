namespace Centuriin.CardGame.Core.Common.GameProfiles;

public interface IGamePipelineBuilder : IConfigurableGamePipeline
{
    public void Build();
}