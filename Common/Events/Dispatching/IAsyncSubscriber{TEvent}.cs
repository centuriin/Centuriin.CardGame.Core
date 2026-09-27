namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IAsyncSubscriber<TEvent>
{
    public Task OnEventAsync(TEvent @event, CancellationToken token);
}