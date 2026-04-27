using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface ISubscriber<TEvent>
    where TEvent : IGameEvent
{
    public void OnEvent(
        TEvent @event,
        IGameState gameState,
        IGameEventBus writer);
}