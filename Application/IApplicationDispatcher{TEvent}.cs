namespace Centuriin.CardGame.Core.Application;

public interface IApplicationDispatcher<TEvent>
{
    public Task PublishAsync<T>(T @event, CancellationToken token)
        where T : TEvent;
}