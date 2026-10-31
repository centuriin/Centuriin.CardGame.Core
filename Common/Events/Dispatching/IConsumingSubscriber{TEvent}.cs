using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IConsumingSubscriber<TEvent>
    where TEvent : IGameEvent
{
    public void OnEvent(TEvent @event, IGameState gameState);
}