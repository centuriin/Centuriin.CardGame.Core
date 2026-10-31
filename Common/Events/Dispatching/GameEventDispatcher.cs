using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public sealed class GameEventDispatcher : IProducingSubcriberDispatcher, IConsumingSubscriberDispatcher
{
    private bool _disposed;

    private readonly OrderedDictionary<Type, IProducingSubscriberGroup> _producingSubscribers = [];
    private readonly OrderedDictionary<Type, IConsumingSubscriberGroup> _consumingSubscribers = [];

    private readonly IGameState _gameState;
    private readonly IGameEventBusWriter _eventBusWriter;

    public GameEventDispatcher(
        IGameState gameState,
        IGameEventBusWriter eventBusWriter)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(eventBusWriter);
        _eventBusWriter = eventBusWriter;
    }

    /// <inheritdoc/>
    public void Register<TEvent>(IProducingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventType = typeof(TEvent);

        if (_producingSubscribers.TryGetValue(eventType, out var subscribersGroup))
        {
            var group = (ProducingSubscribersGroup<TEvent>)subscribersGroup;
            group.Subscribers.Add(subscriber);
        }
        else
        {
            var group = new ProducingSubscribersGroup<TEvent>();
            group.Subscribers.Add(subscriber);

            _producingSubscribers[eventType] = group;
        }
    }

    /// <inheritdoc/>
    public void Register<TEvent>(IConsumingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventType = typeof(TEvent);

        if (_consumingSubscribers.TryGetValue(eventType, out var subscribersGroup))
        {
            var group = (ConsumingSubscribersGroup<TEvent>)subscribersGroup;
            group.Subscribers.Add(subscriber);
        }
        else
        {
            var group = new ConsumingSubscribersGroup<TEvent>();
            group.Subscribers.Add(subscriber);

            _consumingSubscribers[eventType] = group;
        }
    }

    /// <inheritdoc/>
    public void Unregister<TEvent>(IProducingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventType = typeof(TEvent);

        if (!_producingSubscribers.TryGetValue(eventType, out var subscribersGroup))
        {
            throw new InvalidOperationException(
                $"No subscribers for {eventType.Name} event type.");
        }

        var group = (ProducingSubscribersGroup<TEvent>)subscribersGroup;
        group.Subscribers.Remove(subscriber);

        if (group.Subscribers.Count == 0)
        {
            _producingSubscribers.Remove(eventType);
        }
    }

    /// <inheritdoc/>
    public void Unregister<TEvent>(IConsumingSubscriber<TEvent> subscriber)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var eventType = typeof(TEvent);

        if (!_consumingSubscribers.TryGetValue(eventType, out var subscribersGroup))
        {
            throw new InvalidOperationException(
                $"No subscribers for {eventType.Name} event type.");
        }

        var group = (ConsumingSubscribersGroup<TEvent>)subscribersGroup;
        group.Subscribers.Remove(subscriber);

        if (group.Subscribers.Count == 0)
        {
            _consumingSubscribers.Remove(eventType);
        }
    }

    /// <inheritdoc/>
    void IProducingSubcriberDispatcher.Publish(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var actualType = @event.GetType();

        var groups = _producingSubscribers
            .Where(x => x.Key.IsAssignableFrom(actualType))
            .Select(x => x.Value);

        foreach (var group in groups)
        {
            group.Deliver(@event, _gameState, _eventBusWriter);
        }
    }

    /// <inheritdoc/>
    void IConsumingSubscriberDispatcher.Publish(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        ObjectDisposedException.ThrowIf(_disposed, this);

        var actualType = @event.GetType();

        var groups = _consumingSubscribers
            .Where(x => x.Key.IsAssignableFrom(actualType))
            .Select(x => x.Value);

        foreach (var group in groups)
        {
            group.Deliver(@event, _gameState);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _producingSubscribers.Clear();
        _consumingSubscribers.Clear();

        _disposed = true;
    }

    private interface IProducingSubscriberGroup
    {
        public void Deliver(
            IGameEvent @event,
            IReadOnlyGameState readOnlyGameState,
            IGameEventBusWriter eventBusWriter);
    }

    private interface IConsumingSubscriberGroup
    {
        public void Deliver(
            IGameEvent @event,
            IGameState readOnlyGameState);
    }

    private sealed class ProducingSubscribersGroup<TEvent> : IProducingSubscriberGroup
        where TEvent : IGameEvent
    {
        public List<IProducingSubscriber<TEvent>> Subscribers { get; } = [];

        public void Deliver(
            IGameEvent @event,
            IReadOnlyGameState readOnlyGameState,
            IGameEventBusWriter eventBusWriter)
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.OnEvent((TEvent)@event, readOnlyGameState, eventBusWriter);
            }
        }
    }

    private sealed class ConsumingSubscribersGroup<TEvent> : IConsumingSubscriberGroup
        where TEvent : IGameEvent
    {
        public List<IConsumingSubscriber<TEvent>> Subscribers { get; } = [];

        public void Deliver(
            IGameEvent @event,
            IGameState readOnlyGameState)
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.OnEvent((TEvent)@event, readOnlyGameState);
            }
        }
    }
}