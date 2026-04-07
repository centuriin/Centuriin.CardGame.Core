namespace Centuriin.CardGame.Core.Common.Events;

public interface IPublisher<TEvent>
    where TEvent : IGameEvent
{
}
