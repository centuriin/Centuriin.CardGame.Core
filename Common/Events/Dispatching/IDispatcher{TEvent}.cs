namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IDispatcher<TEvent>
{
    public void Publish(TEvent @event);

    public void Register<T>(ISubscriber<T> subscriber)
        where T : TEvent;

    public void Unregister<T>(ISubscriber<T> subscriber)
        where T : TEvent;
}
