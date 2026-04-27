using Centuriin.CardGame.Core.Common.Entities;

namespace Centuriin.CardGame.Core.Common;

public sealed class TurnAutomat : ITurnAutomat
{
    private List<EntityId> _players = [];
    private LinkedList<EntityId> _playersQueue = [];

    public bool IsCycled => _players.Count != 0;

    public EntityId ActivePlayer => _playersQueue.First?.Value
        ?? throw new InvalidOperationException();

    public void SetCycle(IReadOnlyCollection<EntityId> playerIds)
    {
        ArgumentNullException.ThrowIfNull(playerIds);
        _players = [.. playerIds];
        _playersQueue = new LinkedList<EntityId>(_players);
    }

    public void DropCycle() => _players.Clear();

    public void MoveNext()
    {
        _playersQueue.RemoveFirst();

        FillCycleIfNeeded();
    }

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

    public void DropQueueAfter(EntityId playerId)
    {
        var node = _playersQueue.Find(playerId)
            ?? throw new InvalidOperationException();

        while (node.Next is not null)
        {
            _playersQueue.Remove(node.Next);
        }
    }

    public IEnumerable<EntityId> GetEnumarable() => _playersQueue;

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