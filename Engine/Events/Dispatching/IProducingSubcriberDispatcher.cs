using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events.Dispatching;

public interface IProducingSubcriberDispatcher
{
    public void Publish(IGameEvent @event);

    public void Register<TEvent>(IProducingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;

    public void Unregister<TEvent>(IProducingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;
}