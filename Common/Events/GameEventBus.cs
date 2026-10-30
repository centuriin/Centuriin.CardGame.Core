using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed class GameEventBus : IGameEventBus
{
    private readonly Queue<IGameEvent> _queue = [];

    public void Clear() => _queue.Clear();

    public bool TryRead(out IGameEvent @event) => _queue.TryDequeue(out @event!);

    public void Write(IGameEvent @event) => _queue.Enqueue(@event);
}