using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public sealed class EventDispatcher : IEventDispatcher
{
    private bool _disposed;

    private readonly Dictionary<Type, Action<IGameEvent, IGameState, IEventBusWriter>> _handlersMap = [];
    private readonly Dictionary<Delegate, Action<IGameEvent, IGameState, IEventBusWriter>> _wrappersMap = [];

    /// <inheritdoc/>
    public void Register<TEvent>(ISubscriber<TEvent> subscriber)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(subscriber);

        var eventType = typeof(TEvent);

        var wrapper = (IGameEvent e, IGameState s, IEventBusWriter w) =>
            subscriber.OnEvent((TEvent)e, s, w);

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
    public void Publish(
        IGameEvent @event,
        IGameState gameState,
        IEventBusWriter writer)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(gameState);
        ArgumentNullException.ThrowIfNull(writer);

        var actualType = @event.GetType();

        var actions = _handlersMap
            .Where(x => x.Key.IsAssignableFrom(actualType))
            .SelectMany(x => x.Value.GetInvocationList())
            .Cast<Action<IGameEvent, IGameState, IEventBusWriter>>();

        foreach (var action in actions)
        {
            action(@event, gameState, writer);
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