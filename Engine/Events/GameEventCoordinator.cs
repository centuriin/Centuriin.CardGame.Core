using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

/// <summary>
/// Applies domain events and coordinates the production of successor events.
/// </summary>
public sealed class GameEventCoordinator : IGameEventProducer, IGameEventApplier
{
    private readonly IGameEventBus _eventBus;
    private readonly IProducingSubscriberDispatcher _producingDispatcher;
    private readonly IConsumingSubscriberDispatcher _consumingSubscriberDispatcher;

    /// <summary>
    /// Initializes a new <see cref="GameEventCoordinator"/> instance.
    /// </summary>
    /// <param name="eventBus">
    /// The queue used to read and write domain events.
    /// </param>
    /// <param name="producingDispatcher">
    /// The dispatcher used to invoke event-producing subscribers.
    /// </param>
    /// <param name="consumingSubscriberDispatcher">
    /// The dispatcher used to invoke state-changing subscribers.
    /// </param>
    public GameEventCoordinator(
        IGameEventBus eventBus,
        IProducingSubscriberDispatcher producingDispatcher,
        IConsumingSubscriberDispatcher consumingSubscriberDispatcher)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        _eventBus = eventBus;

        ArgumentNullException.ThrowIfNull(producingDispatcher);
        _producingDispatcher = producingDispatcher;

        ArgumentNullException.ThrowIfNull(consumingSubscriberDispatcher);
        _consumingSubscriberDispatcher = consumingSubscriberDispatcher;
    }

    /// <inheritdoc/>
    public void Apply(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        _consumingSubscriberDispatcher.Publish(@event);
    }

    /// <inheritdoc/>
    public IReadOnlyList<IGameEvent> Produce(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var applied = new List<IGameEvent>();

        do
        {
            _consumingSubscriberDispatcher.Publish(@event);
            _producingDispatcher.Publish(@event);

            applied.Add(@event);
        }
        while (_eventBus.TryRead(out @event));

        return applied;
    }
}
