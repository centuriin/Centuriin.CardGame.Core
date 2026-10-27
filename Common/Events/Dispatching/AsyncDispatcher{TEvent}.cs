namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public sealed class AsyncDispatcher<TEvent> : IAsyncDispatcher<TEvent>, IAsyncDisposable
{
    private bool _disposed;

    private readonly OrderedDictionary<Type, ISubscribersGroup> _handlersMap = [];

    /// <inheritdoc/>
    public void Register<T>(IAsyncSubscriber<T> subscriber)
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
    public void Unregister<T>(IAsyncSubscriber<T> subscriber)
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
    public async Task PublishAsync(TEvent @event, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var actualType = @event.GetType();

        var groups = _handlersMap
            .Where(x => x.Key.IsAssignableFrom(actualType))
            .Select(x => x.Value);

        foreach (var group in groups)
        {
            await group.DeliverAsync(@event, token);
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
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
        public Task DeliverAsync(TEvent @event, CancellationToken token);
    }

    private sealed class SubscribersGroup<T> : ISubscribersGroup
        where T : TEvent
    {
        public List<IAsyncSubscriber<T>> Subscribers { get; } = [];

        public async Task DeliverAsync(TEvent @event, CancellationToken token)
        {
            foreach (var subscriber in Subscribers)
            {
                await subscriber.OnEventAsync((T)@event!, token);
            }
        }
    }
}
