namespace Centuriin.CardGame.Core.Common.Events;

public interface IEventDispatcher
{
    public void Publish(
        IGameEvent @event,
        IGameState gameState,
        IEventBusWriter writer);

    public void Register<TEvent>(
        ISubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;

    public void Unregister<TEvent>(
        ISubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;
}
