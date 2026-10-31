using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events;

public interface IGameEventProducer
{
    public IReadOnlyList<IGameEvent> Produce(IGameEvent @event);
}
