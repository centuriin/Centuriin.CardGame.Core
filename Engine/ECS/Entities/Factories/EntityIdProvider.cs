namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

/// <summary>
/// Allocates sequential entity identifiers within a game.
/// </summary>
public sealed class EntityIdProvider : IEntityIdProvider
{
    private EntityId _currentId = EntityId.Default;

    /// <inheritdoc/>
    public EntityId GetNextId() => ++_currentId;

    /// <inheritdoc/>
    public void SetInitialId(EntityId id) => _currentId = id;
}