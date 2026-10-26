using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Server;

public sealed class GameEventApplier : IGameEventApplier
{
    private readonly IGameEventBus _eventBus;
    private readonly IDispatcher<IGameEvent> _gameEventDispatcher;

    public GameEventApplier(
        IGameEventBus eventBus,
        IDispatcher<IGameEvent> gameEventDispatcher)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        _eventBus = eventBus;

        ArgumentNullException.ThrowIfNull(gameEventDispatcher);
        _gameEventDispatcher = gameEventDispatcher;
    }

    public IGameEventsBatch Apply(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var eventsBatch = new List<IGameEvent>()
        {
            @event
        };

        _gameEventDispatcher.Publish(@event);

        while (_eventBus.TryRead(out var nextEvent))
        {
            _gameEventDispatcher.Publish(nextEvent);
            eventsBatch.Add(nextEvent);
        }

        return new GameEventsBatch(eventsBatch);
    }

    private sealed class GameEventsBatch : IGameEventsBatch
    {
        public IReadOnlyList<IGameEvent> Events { get; }

        public GameEventsBatch(IReadOnlyList<IGameEvent> gameEvents)
        {
            ArgumentNullException.ThrowIfNull(gameEvents);
            Events = gameEvents;
        }
    }
}
