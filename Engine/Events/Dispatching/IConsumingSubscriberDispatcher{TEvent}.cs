using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events.Dispatching;

public interface IConsumingSubscriberDispatcher
{
    public void Publish(IGameEvent @event);

    public void Register<TEvent>(IConsumingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;

    public void Unregister<TEvent>(IConsumingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;
}
