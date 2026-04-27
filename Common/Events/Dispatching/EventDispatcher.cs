namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public sealed class EventDispatcher : IEventDispatcher
{
    private bool _disposed;

    private readonly Dictionary<Type, Action<IGameEvent>> _handlersMap = [];
    private readonly Dictionary<Delegate, Action<IGameEvent>> _wrappersMap = [];

    /// <inheritdoc/>
    public void Register<TEvent>(ISubscriber<TEvent> subscriber)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventType = typeof(TEvent);

        var wrapper = (IGameEvent e) => subscriber.OnEvent((TEvent)e);

        if (_handlersMap.TryGetValue(eventType, out var actions))
        {
            actions += wrapper;
            _handlersMap[eventType] = actions;
        }
        else
        {
            _handlersMap[eventType] = wrapper;
        }

        _wrappersMap[subscriber.OnEvent] = wrapper;
    }

    /// <inheritdoc/>
    public void Unregister<TEvent>(ISubscriber<TEvent> subscriber)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventType = typeof(TEvent);

        if (!_wrappersMap.TryGetValue(subscriber.OnEvent, out var wrapper))
        {
            throw new InvalidOperationException();
        }

        if (!_handlersMap.TryGetValue(eventType, out var actions))
        {
            throw new InvalidOperationException();
        }

        actions -= wrapper;

        if (actions is null)
        {
            _handlersMap.Remove(eventType);
        }
        else
        {
            _handlersMap[eventType] = actions;
        }

        _wrappersMap.Remove(subscriber.OnEvent);
    }

    /// <inheritdoc/>
    public void Publish(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var actualType = @event.GetType();

        var actions = _handlersMap
            .Where(x => x.Key.IsAssignableFrom(actualType))
            .SelectMany(x => x.Value.GetInvocationList())
            .Cast<Action<IGameEvent>>();

        foreach (var action in actions)
        {
            action(@event);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _wrappersMap.Clear();
        _handlersMap.Clear();

        _disposed = true;
    }
}