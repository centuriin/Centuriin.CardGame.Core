using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

public interface IConfigurableSystem
{
    public IConfigurableSystem ForEvent<TEvent>()
        where TEvent : IGameEvent;

    public IGamePipelineConfigurator GamePipeline { get; }
}