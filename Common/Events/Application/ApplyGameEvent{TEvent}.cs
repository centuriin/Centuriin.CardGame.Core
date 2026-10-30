using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class ApplyGameEvent<TEvent> : IApplicationEvent
    where TEvent : IGameEvent
{
    public IGameEvent Event { get; }

    public IGameState MutableGameState { get; }

    public ApplyGameEvent(IGameEvent @event, IGameState mutableGameState)
    {
        ArgumentNullException.ThrowIfNull(@event);
        Event = @event;

        ArgumentNullException.ThrowIfNull(mutableGameState);
        MutableGameState = mutableGameState;
    }
}
