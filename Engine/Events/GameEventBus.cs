using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

public sealed class GameEventBus : IGameEventBus
{
    private readonly Queue<IGameEvent> _queue = [];

    public bool TryRead(out IGameEvent @event) => _queue.TryDequeue(out @event!);

    public void Write(IGameEvent @event) => _queue.Enqueue(@event);
}