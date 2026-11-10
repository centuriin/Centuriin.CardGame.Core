using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Dispatching;

public interface IConsumingSubscriber<TEvent>
    where TEvent : IGameEvent
{
    public void OnEvent(TEvent @event, IGameState gameState);
}