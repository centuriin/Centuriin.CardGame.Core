using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IConsumingSubscriberDispatcher
{
    public void Publish(IGameEvent @event);

    public void Register<TEvent>(IConsumingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;

    public void Unregister<TEvent>(IConsumingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;
}
