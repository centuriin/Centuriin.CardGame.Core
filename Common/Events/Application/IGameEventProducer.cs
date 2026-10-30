using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public interface IGameEventProducer
{
    public IReadOnlyList<IGameEvent> Produce<TEvent>(TEvent produceEvent)
        where TEvent : IGameEvent;
}
