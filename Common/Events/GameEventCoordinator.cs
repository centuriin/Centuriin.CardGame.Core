using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed class GameEventCoordinator : IGameEventProducer, IGameEventApplier
{
    private readonly IGameEventBus _eventBus;
    private readonly IProducingSubcriberDispatcher _producingDispatcher;
    private readonly IConsumingSubscriberDispatcher _consumingSubscriberDispatcher;

    public GameEventCoordinator(
        IGameEventBus eventBus,
        IProducingSubcriberDispatcher producingDispatcher,
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
