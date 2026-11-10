using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

public interface IConfigurableSystem
{
    public IConfigurableGamePipeline Pipeline { get; }

    public IConfigurableSystem ForEvent<TEvent>()
        where TEvent : IGameEvent;
}