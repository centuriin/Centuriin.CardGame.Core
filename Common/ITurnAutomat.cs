using Centuriin.CardGame.Core.Common.Entities.Players;

namespace Centuriin.CardGame.Core.Common;

public interface ITurnAutomat
{
    public PlayerId ActivePlayer { get; }

    public bool IsCycled { get; }


    public void DropCycle();

    public void DropQueueAfter(PlayerId playerId);

    public IEnumerable<PlayerId> GetEnumarable();

    public void MoveNext();

    public void SetCycle(IReadOnlyCollection<PlayerId> playerIds);

    public void SetNext(params IReadOnlyCollection<PlayerId> players);
}