namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IAsyncDispatcher<TEvent>
{
    public Task PublishAsync(TEvent @event, CancellationToken token);

    public void Register<T>(IAsyncSubscriber<T> subscriber)
        where T : TEvent;

    public void Unregister<T>(IAsyncSubscriber<T> subscriber)
        where T : TEvent;
}