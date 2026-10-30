using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Root;

public sealed class GameEventApplier
{
    private readonly IGameEventBus _eventBus;
    private readonly IDispatcher<IGameEvent> _dispatcher;

    public GameEventApplier(
        IGameEventBus eventBus,
        IDispatcher<IGameEvent> dispatcher)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        _eventBus = eventBus;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;
    }

    public IReadOnlyList<IGameEvent> Apply(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var appliedEvents = new List<IGameEvent>()
        {
            @event
        };

        _dispatcher.Publish(@event);

        while (_eventBus.TryRead(out var nextEvent))
        {
            _dispatcher.Publish(nextEvent);
            appliedEvents.Add(nextEvent);
        }

        return appliedEvents;
    }

    public void SilentApply(IReadOnlyList<IGameEvent> batch)
    {
        ArgumentNullException.ThrowIfNull(batch);

        foreach (var @event in batch)
        {
            _dispatcher.Publish(@event);
        }

        // Make sure that event bus is empty
        _eventBus.Clear();
    }
}
