using Centuriin.CardGame.Core.Common.Entities.Players;

namespace Centuriin.CardGame.Core.Common;

public sealed class TurnAutomat : ITurnAutomat
{
    private List<PlayerId> _players = [];
    private LinkedList<PlayerId> _playersQueue = [];

    public bool IsCycled => _players.Count != 0;

    public PlayerId ActivePlayer => _playersQueue.First?.Value
        ?? throw new InvalidOperationException();

    public void SetCycle(IReadOnlyCollection<PlayerId> playerIds)
    {
        ArgumentNullException.ThrowIfNull(playerIds);
        _players = [.. playerIds];
        _playersQueue = new LinkedList<PlayerId>(_players);
    }

    public void DropCycle() => _players.Clear();

    public void MoveNext()
    {
        _playersQueue.RemoveFirst();

        FillCycleIfNeeded();
    }

    public void SetNext(params IReadOnlyCollection<PlayerId> players)
    {
        ArgumentNullException.ThrowIfNull(players);

        if (players.Count == 0)
        {
            throw new InvalidOperationException();
        }

        IEnumerable<PlayerId> playersTemp = players;
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

    public void DropQueueAfter(PlayerId playerId)
    {
        var node = _playersQueue.Find(playerId)
            ?? throw new InvalidOperationException();

        while (node.Next is not null)
        {
            _playersQueue.Remove(node.Next);
        }
    }

    public IEnumerable<PlayerId> GetEnumarable() => _playersQueue;

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