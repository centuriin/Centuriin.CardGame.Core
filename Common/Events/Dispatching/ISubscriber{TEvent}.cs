namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface ISubscriber<TEvent>
{
    public void OnEvent(TEvent @event);
}