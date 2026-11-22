namespace Centuriin.CardGame.Core.Application;

public interface IApplicationHandler<in TEvent>
{
    public Task HandleAsync(TEvent @event, CancellationToken token);
}
