using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common;

public interface ITurnAutomat : IReadOnlyTurnAutomat
{
    public void DropCycle();

    public void DropQueueAfter(EntityId playerId);

    public void MoveNext();

    public void SetCycle(IReadOnlyCollection<EntityId> playerIds);

    public void SetNext(params IReadOnlyCollection<EntityId> players);
}