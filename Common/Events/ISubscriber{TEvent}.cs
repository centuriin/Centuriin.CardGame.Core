namespace Centuriin.CardGame.Core.Common.Events;

public interface ISubscriber<TEvent>
    where TEvent : IGameEvent
{
    public Task OnEvent(TEvent @event, IGameState gameState, CancellationToken token);
}