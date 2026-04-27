using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IEventDispatcher : IDisposable
{
    public void Publish(
        IGameEvent @event,
        IGameState gameState,
        IGameEventBus writer);

    public void Register<TEvent>(
        ISubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;

    public void Unregister<TEvent>(
        ISubscriber<TEvent> subscriber)
        where TEvent : IGameEvent;
}
