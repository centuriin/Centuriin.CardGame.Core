namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public sealed class Dispatcher<TEvent> : IDispatcher<TEvent>, IDisposable
{
    private bool _disposed;

    private readonly Dictionary<Type, ISubscribersGroup> _handlersMap = [];

    /// <inheritdoc/>
    public void Register<T>(ISubscriber<T> subscriber)
        where T : TEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventType = typeof(T);

        if (_handlersMap.TryGetValue(eventType, out var subscribersGroup))
        {
            var group = (SubscribersGroup<T>)subscribersGroup;
            group.Subscribers.Add(subscriber);
        }
        else
        {
            var group = new SubscribersGroup<T>();
            group.Subscribers.Add(subscriber);

            _handlersMap[eventType] = group;
        }
    }

    /// <inheritdoc/>
    public void Unregister<T>(ISubscriber<T> subscriber)
        where T : TEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventType = typeof(T);

        if (!_handlersMap.TryGetValue(eventType, out var subscribersGroup))
        {
            throw new InvalidOperationException(
                $"No subscribers for {eventType.Name} event type.");
        }

        var group = (SubscribersGroup<T>)subscribersGroup;
        group.Subscribers.Remove(subscriber);

        if (group.Subscribers.Count == 0)
        {
            _handlersMap.Remove(eventType);
        }
    }

    /// <inheritdoc/>
    public void Publish(TEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var actualType = @event.GetType();

        var groups = _handlersMap
            .Where(x => x.Key.IsAssignableFrom(actualType))
            .Select(x => x.Value);

        foreach (var group in groups)
        {
            group.Deliver(@event);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _handlersMap.Clear();

        _disposed = true;
    }

    private interface ISubscribersGroup
    {
        public void Deliver(TEvent @event);
    }

    private sealed class SubscribersGroup<T> : ISubscribersGroup
        where T : TEvent
    {
        public List<ISubscriber<T>> Subscribers { get; } = [];

        public void Deliver(TEvent @event)
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.OnEvent((T)@event!);
            }
        }
    }
}
