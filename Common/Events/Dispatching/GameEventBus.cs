using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public sealed class GameEventBus : IGameEventBus
{
    private readonly Queue<IGameEvent> _queue = [];

    public bool TryRead(out IGameEvent @event) => _queue.TryDequeue(out @event!);

    public void Write(IGameEvent @event) => _queue.Enqueue(@event);
}