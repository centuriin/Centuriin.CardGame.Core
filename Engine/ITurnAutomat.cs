using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

public interface ITurnAutomat : IReadOnlyTurnAutomat
{
    public void DropCycle();

    public void DropQueueAfter(EntityId playerId);

    public void MoveNext();

    public void SetCycle(IReadOnlyCollection<EntityId> playerIds);

    public void SetNext(params IReadOnlyCollection<EntityId> players);
}