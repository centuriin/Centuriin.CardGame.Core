using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

/// <summary>
/// Queues domain events in first-in, first-out order within a game.
/// </summary>
public sealed class GameEventBus : IGameEventBus
{
    private readonly Queue<IGameEvent> _queue = [];

    /// <inheritdoc/>
    public bool TryRead(out IGameEvent @event) => _queue.TryDequeue(out @event!);

    /// <inheritdoc/>
    public void Write(IGameEvent @event) => _queue.Enqueue(@event);
}