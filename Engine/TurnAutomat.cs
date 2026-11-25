using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Maintains the active player, queued turns, and an optional repeating player cycle.
/// </summary>
public sealed class TurnAutomat : ITurnAutomat
{
    private List<EntityId> _players = [];
    private LinkedList<EntityId> _playersQueue = [];

    /// <inheritdoc/>
    public bool IsCycled => _players.Count != 0;

    /// <inheritdoc/>
    public EntityId ActivePlayer => _playersQueue.First?.Value
        ?? throw new InvalidOperationException();

    /// <inheritdoc/>
    public void SetCycle(IReadOnlyCollection<EntityId> playerIds)
    {
        ArgumentNullException.ThrowIfNull(playerIds);
        _players = [.. playerIds];
        _playersQueue = new LinkedList<EntityId>(_players);
    }

    /// <inheritdoc/>
    public void DropCycle() => _players.Clear();

    /// <inheritdoc/>
    public void MoveNext()
    {
        _playersQueue.RemoveFirst();

        FillCycleIfNeeded();
    }

    /// <inheritdoc/>
    public void SetNext(params IReadOnlyCollection<EntityId> players)
    {
        ArgumentNullException.ThrowIfNull(players);

        if (players.Count == 0)
        {
            throw new InvalidOperationException();
        }

        IEnumerable<EntityId> playersTemp = players;
        if (_playersQueue.Count == 0)
        {
            _playersQueue.AddFirst(playersTemp.First());
            playersTemp = playersTemp.Skip(1);
        }

        var currentNode = _playersQueue.First!;

        foreach (var p in playersTemp)
        {
            currentNode = _playersQueue.AddAfter(currentNode, p);
        }
    }

    /// <inheritdoc/>
    public void DropQueueAfter(EntityId playerId)
    {
        var node = _playersQueue.Find(playerId)
            ?? throw new InvalidOperationException();

        while (node.Next is not null)
        {
            _playersQueue.Remove(node.Next);
        }
    }

    /// <inheritdoc/>
    public IEnumerable<EntityId> GetEnumerable() => _playersQueue;

    private void FillCycleIfNeeded()
    {
        if (_playersQueue.Count == 0 && IsCycled)
        {
            foreach (var p in _players)
            {
                _playersQueue.AddLast(p);
            }
        }
    }
}