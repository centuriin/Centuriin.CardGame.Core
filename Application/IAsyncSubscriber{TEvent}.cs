namespace Centuriin.CardGame.Core.Application;

public interface IAsyncSubscriber<TEvent>
{
    public Task OnEventAsync(TEvent @event, CancellationToken token);
}