using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Observability.Logging;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Dispatching;

/// <summary>
/// Delivers domain events to registered state-changing and event-producing subscribers.
/// </summary>
public sealed class GameEventDispatcher : IProducingSubscriberDispatcher, IConsumingSubscriberDispatcher
{
    private bool _disposed;

    private readonly OrderedDictionary<Type, IProducingSubscriberGroup> _producingSubscribers = [];
    private readonly OrderedDictionary<Type, IConsumingSubscriberGroup> _consumingSubscribers = [];

    private readonly IGameState _gameState;
    private readonly IGameEventBusWriter _eventBusWriter;
    private readonly ICoreLogger<GameEventDispatcher> _logger;

    /// <summary>
    /// Initializes a new <see cref="GameEventDispatcher"/> instance.
    /// </summary>
    /// <param name="gameState">
    /// The game state used by the operation.
    /// </param>
    /// <param name="eventBusWriter">
    /// The queue writer used to publish successor events.
    /// </param>
    /// <param name="logger">
    /// The logger used for diagnostics.
    /// </param>
    public GameEventDispatcher(
        IGameState gameState,
        IGameEventBusWriter eventBusWriter,
        ICoreLogger<GameEventDispatcher> logger)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(eventBusWriter);
        _eventBusWriter = eventBusWriter;

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
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
            var group = new ProducingSubscribersGroup<TEvent> { Logger = _logger };
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
            var group = new ConsumingSubscribersGroup<TEvent> { Logger = _logger };
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
    void IProducingSubscriberDispatcher.Publish(IGameEvent @event)
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

    /// <summary>
    /// Clears event subscriptions and prevents further use of the dispatcher.
    /// </summary>
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

    /// <summary>
    /// Delivers an event to a group of event-producing subscribers.
    /// </summary>
    private interface IProducingSubscriberGroup
    {
        /// <summary>
        /// Delivers the event to this group's subscribers in registration order.
        /// </summary>
        /// <param name="event">
        /// The domain or application event to process.
        /// </param>
        /// <param name="readOnlyGameState">
        /// The current game state available for reading.
        /// </param>
        /// <param name="eventBusWriter">
        /// The queue writer used to publish successor events.
        /// </param>
        public void Deliver(
            IGameEvent @event,
            IReadOnlyGameState readOnlyGameState,
            IGameEventBusWriter eventBusWriter);
    }

    /// <summary>
    /// Delivers an event to a group of state-changing subscribers.
    /// </summary>
    private interface IConsumingSubscriberGroup
    {
        /// <summary>
        /// Delivers the event to this group's subscribers in registration order.
        /// </summary>
        /// <param name="event">
        /// The domain or application event to process.
        /// </param>
        /// <param name="readOnlyGameState">
        /// The current game state available for reading.
        /// </param>
        public void Deliver(
            IGameEvent @event,
            IGameState readOnlyGameState);
    }

    /// <summary>
    /// Stores and invokes event-producing subscribers for an event type.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The event type handled by this contract.
    /// </typeparam>
    private sealed class ProducingSubscribersGroup<TEvent> : IProducingSubscriberGroup
        where TEvent : IGameEvent
    {
        /// <summary>
        /// Gets the logger used when delivering events.
        /// </summary>
        public required ICoreLogger Logger { get; init; }

        /// <summary>
        /// Gets the subscribers in delivery order.
        /// </summary>
        public List<IProducingSubscriber<TEvent>> Subscribers { get; } = [];

        /// <inheritdoc/>
        public void Deliver(
            IGameEvent @event,
            IReadOnlyGameState readOnlyGameState,
            IGameEventBusWriter eventBusWriter)
        {
            foreach (var subscriber in Subscribers)
            {
                if (Logger.IsEnabled(LogLevel.Debug))
                {
                    Logger.LogDebug(
                        "System {SystemType} reacts on event {EventType}",
                        subscriber.GetType().Name,
                        @event.GetType().Name);
                }

                subscriber.OnEvent((TEvent)@event, readOnlyGameState, eventBusWriter);
            }
        }
    }

    /// <summary>
    /// Stores and invokes state-changing subscribers for an event type.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The event type handled by this contract.
    /// </typeparam>
    private sealed class ConsumingSubscribersGroup<TEvent> : IConsumingSubscriberGroup
        where TEvent : IGameEvent
    {
        /// <summary>
        /// Gets the logger used when delivering events.
        /// </summary>
        public required ICoreLogger Logger { get; init; }

        /// <summary>
        /// Gets the subscribers in delivery order.
        /// </summary>
        public List<IConsumingSubscriber<TEvent>> Subscribers { get; } = [];

        /// <inheritdoc/>
        public void Deliver(
            IGameEvent @event,
            IGameState readOnlyGameState)
        {
            foreach (var subscriber in Subscribers)
            {
                if (Logger.IsEnabled(LogLevel.Debug))
                {
                    Logger.LogDebug(
                        "System {SystemType} applies {EventType}",
                        subscriber.GetType().Name,
                        @event.GetType().Name);
                }

                subscriber.OnEvent((TEvent)@event, readOnlyGameState);
            }
        }
    }
}