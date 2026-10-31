using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common;

public interface IReadOnlyTurnAutomat
{
    public EntityId ActivePlayer { get; }

    public bool IsCycled { get; }

    public IEnumerable<EntityId> GetEnumarable();
}
