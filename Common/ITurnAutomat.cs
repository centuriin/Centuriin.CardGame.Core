using Centuriin.CardGame.Core.Common.Entities;

namespace Centuriin.CardGame.Core.Common;

public interface ITurnAutomat
{
    public EntityId ActivePlayer { get; }

    public bool IsCycled { get; }


    public void DropCycle();

    public void DropQueueAfter(EntityId playerId);

    public IEnumerable<EntityId> GetEnumarable();

    public void MoveNext();

    public void SetCycle(IReadOnlyCollection<EntityId> playerIds);

    public void SetNext(params IReadOnlyCollection<EntityId> players);
}