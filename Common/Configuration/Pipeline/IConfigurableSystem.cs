using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

public interface IConfigurableSystem
{
    public IConfigurableGamePipeline GamePipeline { get; }

    public IConfigurableSystem ForEvent<TEvent>()
        where TEvent : IGameEvent;
}