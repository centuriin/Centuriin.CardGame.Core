namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public interface IEntityIdProvider
{
    public EntityId GetNextId();

    public void SetInitialId(EntityId id);
}
