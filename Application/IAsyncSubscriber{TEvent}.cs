namespace Centuriin.CardGame.Core.Engine.Events.Dispatching;

public interface IAsyncSubscriber<TEvent>
{
    public Task OnEventAsync(TEvent @event, CancellationToken token);
}