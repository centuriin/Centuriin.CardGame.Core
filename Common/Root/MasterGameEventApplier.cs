using System.Collections;

using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Root;

public sealed class MasterGameEventApplier : GameEventApplierBase
{
    private readonly IGameEventBus _eventBus;

    public MasterGameEventApplier(
        IGameEventBus eventBus,
        IDispatcher<IGameEvent> dispatcher)
        : base(dispatcher)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        _eventBus = eventBus;
    }

    public override IGameEventsBatch Apply(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var eventsBatch = new List<IGameEvent>()
        {
            @event
        };

        Dispatcher.Publish(@event);

        while (_eventBus.TryRead(out var nextEvent))
        {
            Dispatcher.Publish(nextEvent);
            eventsBatch.Add(nextEvent);
        }

        return new GameEventsBatch(eventsBatch);
    }

    public override void SilentApply(IGameEventsBatch batch)
    {
        base.SilentApply(batch);

        // Make sure that event bus is empty
        _eventBus.Clear();
    }

    private sealed class GameEventsBatch : IGameEventsBatch
    {
        public IReadOnlyList<IGameEvent> Events { get; }

        public GameEventsBatch(IReadOnlyList<IGameEvent> gameEvents)
        {
            ArgumentNullException.ThrowIfNull(gameEvents);
            Events = gameEvents;
        }

        public IEnumerator<IGameEvent> GetEnumerator() => Events.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
