namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public sealed class EntityIdProvider : IEntityIdProvider
{
    private EntityId _currentId = EntityId.Default;

    public EntityId GetNextId() => ++_currentId;

    public void SetInitialId(EntityId id) => _currentId = id;
}