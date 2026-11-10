using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

public interface IGameEventProducer
{
    public IReadOnlyList<IGameEvent> Produce(IGameEvent @event);
}
