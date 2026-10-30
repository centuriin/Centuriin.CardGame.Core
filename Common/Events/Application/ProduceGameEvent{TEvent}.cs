using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class ProduceGameEvent<TEvent> : IApplicationEvent
    where TEvent : IGameEvent
{
    public TEvent Event { get; }

    public IReadOnlyGameState ImmutableGameState { get; }

    public IGameEventBusWriter EventBusWriter { get; }

    public ProduceGameEvent(
        TEvent @event,
        IReadOnlyGameState immutableGameState,
        IGameEventBusWriter eventBusWriter)
    {
        ArgumentNullException.ThrowIfNull(@event);
        Event = @event;

        ArgumentNullException.ThrowIfNull(immutableGameState);
        ImmutableGameState = immutableGameState;

        ArgumentNullException.ThrowIfNull(eventBusWriter);
        EventBusWriter = eventBusWriter;
    }
}
