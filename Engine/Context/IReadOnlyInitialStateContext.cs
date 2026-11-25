using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Context;

/// <summary>
/// Exposes initial entities and their last allocated identifier.
/// </summary>
public interface IReadOnlyInitialStateContext : IGameContext
{
    /// <summary>
    /// Gets the last allocated entity identifier.
    /// </summary>
    public EntityId LastEntityId { get; }

    /// <summary>
    /// Gets the entities available in this state or context.
    /// </summary>
    public IReadOnlyEntitiesCollection Entities { get; }
}
