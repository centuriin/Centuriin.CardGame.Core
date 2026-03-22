using Centuriin.CardGame.Core.Common.Events.System;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed class EventDispatcher : IEventDispatcher
{
    private readonly Dictionary<Type, Func<ISystemEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>>> _handlersMap = [];
    private readonly Dictionary<Delegate, Func<ISystemEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>>> _wrappedDelegatesMap = [];

    public void Register<TEvent>(
        Func<TEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> func)
        where TEvent : ISystemEvent
    {
        ArgumentNullException.ThrowIfNull(func);

        var type = typeof(TEvent);

        var wrapper = (ISystemEvent e, CancellationToken token) => func((TEvent)e, token);

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
        Func<TEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>> func)
        where TEvent : ISystemEvent
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

    public async Task PublishAsync(ISystemEvent @event, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        token.ThrowIfCancellationRequested();

        var type = @event.GetType();

        var tasks = _handlersMap
            .Where(x => x.Key.IsAssignableFrom(type))
            .SelectMany(x => x.Value.GetInvocationList())
            .Cast<Func<ISystemEvent, CancellationToken, Task<IReadOnlyCollection<ISystemEvent>>>>()
            .ToList();

        foreach (var task in tasks)
        {
            var @events = await task(@event, token);

            foreach (var e in @events)
            {
                await PublishAsync(e, token);
            }
        }
    }
}
