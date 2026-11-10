using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

public interface IReadOnlyTurnAutomat
{
    public EntityId ActivePlayer { get; }

    public bool IsCycled { get; }

    public IEnumerable<EntityId> GetEnumarable();
}
