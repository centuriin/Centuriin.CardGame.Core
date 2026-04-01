using System.Threading.Channels;

namespace Centuriin.CardGame.Core.Common.Events;

public interface IEventDispatcher<TEventBase>
{
    public Task PublishAsync(
        TEventBase @event,
        ChannelWriter<TEventBase> writer,
        CancellationToken token);

    public void Register<TEvent>(
        Func<TEvent, CancellationToken, Task<IReadOnlyCollection<TEventBase>>> func)
        where TEvent : TEventBase;

    public void Unregister<TEvent>(
        Func<TEvent, CancellationToken, Task<IReadOnlyCollection<TEventBase>>> func)
        where TEvent : TEventBase;
}
