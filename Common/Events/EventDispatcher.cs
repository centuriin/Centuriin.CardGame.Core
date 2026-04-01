using System.Threading.Channels;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed class EventDispatcher : IEventDispatcher<IGameEvent>
{
    private readonly Dictionary<Type, Func<IGameEvent, CancellationToken, Task<IReadOnlyCollection<IGameEvent>>>> _handlersMap = [];
    private readonly Dictionary<Delegate, Func<IGameEvent, CancellationToken, Task<IReadOnlyCollection<IGameEvent>>>> _wrappedDelegatesMap = [];

    public void Register<TEvent>(
        Func<TEvent, CancellationToken, Task<IReadOnlyCollection<IGameEvent>>> func)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(func);

        var type = typeof(TEvent);

        var wrapper = (IGameEvent e, CancellationToken token) => func((TEvent)e, token);

        if (_handlersMap.TryGetValue(type, out var @delegate))
        {
            @delegate += wrapper;

            _handlersMap[type] = @delegate;
        }
        else
        {
            _handlersMap[typeof(TEvent)] = wrapper;
        }

        _wrappedDelegatesMap[func] = wrapper;
    }

    public void Unregister<TEvent>(
        Func<TEvent, CancellationToken, Task<IReadOnlyCollection<IGameEvent>>> func)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(func);

        var type = typeof(TEvent);

        if (!_wrappedDelegatesMap.TryGetValue(func, out var wrapper))
        {
            throw new InvalidOperationException();
        }

        if (!_handlersMap.TryGetValue(type, out var @delegate))
        {
            throw new InvalidOperationException();
        }

        @delegate -= wrapper;

        if (@delegate is null)
        {
            _handlersMap.Remove(type);
        }
        else
        {
            _handlersMap[type] = @delegate;
        }

        _wrappedDelegatesMap.Remove(func);
    }

    public async Task PublishAsync(
        IGameEvent @event, 
        ChannelWriter<IGameEvent> writer,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        token.ThrowIfCancellationRequested();

        var type = @event.GetType();

        var tasks = _handlersMap
            .Where(x => x.Key.IsAssignableFrom(type))
            .SelectMany(x => x.Value.GetInvocationList())
            .Cast<Func<IGameEvent, CancellationToken, Task<IReadOnlyCollection<IGameEvent>>>>()
            .ToList();

        foreach (var task in tasks)
        {
            var @events = await task(@event, token);

            foreach (var e in @events)
            {
                await PublishAsync(e, writer, token);
            }
        }
    }
}
