using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common.World;

public sealed class GameEventApplier : IGameEventApplier
{
    private readonly IGameEventBus _eventBus;
    private readonly IEventDispatcher _dispatcher;
    private readonly IGameEventsRepository _eventsRepository;

    public GameEventApplier(
        IGameEventBus eventBus,
        IEventDispatcher dispatcher,
        IGameEventsRepository eventsRepository)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        _eventBus = eventBus;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;

        ArgumentNullException.ThrowIfNull(eventsRepository);
        _eventsRepository = eventsRepository;
    }

    public async Task<IGameEventUnit> ApplyAsync(IPrimaryEvent @event, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        token.ThrowIfCancellationRequested();

        var randomEvents = new List<IRandomEvent>();

        _dispatcher.Publish(@event);

        while (_eventBus.TryRead(out var nextEvent))
        {
            if (nextEvent is IRandomEvent randomEvent)
            {
                randomEvents.Add(randomEvent);
            }

            _dispatcher.Publish(nextEvent);
        }

        var unit = new EventUnit(@event, randomEvents);

        await _eventsRepository.AddAsync(unit, token);

        return unit;
    }

    private sealed record class EventUnit(
        IPrimaryEvent PrimaryEvent,
        IReadOnlyCollection<IRandomEvent> RelatedRandomEvents) : IGameEventUnit;
}