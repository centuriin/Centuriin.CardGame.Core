using System.Threading.Channels;

namespace Centuriin.CardGame.Core.Common.Events;

public interface ISubscriber<TEvent>
    where TEvent : IGameEvent
{
    public void OnEvent(
        TEvent @event, 
        IGameState gameState, 
        ChannelWriter<IGameEvent> writer);
}