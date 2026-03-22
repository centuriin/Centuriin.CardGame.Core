using Centuriin.CardGame.Core.Common.Events.System;

namespace Centuriin.CardGame.Core.Common.Events;

public interface IEventDispatcher
{
    public Task PublishAsync(ISystemEvent @event, CancellationToken token);

    public void Register<TEvent>(
        Func<TEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> func)
        where TEvent : ISystemEvent;

    public void Unregister<TEvent>(
        Func<TEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> func)
        where TEvent: ISystemEvent;
}
