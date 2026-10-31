using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Observability.Logging;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed class GameEventCoordinator : IGameEventProducer, IGameEventApplier
{
    private readonly IGameEventBus _eventBus;
    private readonly IProducingSubcriberDispatcher _producingDispatcher;
    private readonly IConsumingSubscriberDispatcher _consumingSubscriberDispatcher;
    private readonly ICoreLogger<GameEventCoordinator> _logger;

    public GameEventCoordinator(
        IGameEventBus eventBus,
        IProducingSubcriberDispatcher producingDispatcher,
        IConsumingSubscriberDispatcher consumingSubscriberDispatcher,
        ICoreLogger<GameEventCoordinator> logger)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        _eventBus = eventBus;

        ArgumentNullException.ThrowIfNull(producingDispatcher);
        _producingDispatcher = producingDispatcher;

        ArgumentNullException.ThrowIfNull(consumingSubscriberDispatcher);
        _consumingSubscriberDispatcher = consumingSubscriberDispatcher;

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    public void Apply(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Apply event {@Event}", @event);
        }

        _consumingSubscriberDispatcher.Publish(@event);
    }

    /// <inheritdoc/>
    public IReadOnlyList<IGameEvent> Produce(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Produce event {@Event}", @event);
        }

        var applied = new List<IGameEvent>();

        do
        {
            _consumingSubscriberDispatcher.Publish(@event);
            _producingDispatcher.Publish(@event);

            applied.Add(@event);
        }
        while (_eventBus.TryRead(out var nextEvent));

        return applied;
    }
}
