using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.ECS.Components;

/// <summary>
/// Owner component.
/// </summary>
public sealed record class OwnerComponent : ComponentBase
{
    /// <summary>
    /// Current owner identifier.
    /// </summary>
    public EntityId CurrentOwnerId { get; private set; }

    public OwnerComponent(EntityId ownerId)
    {
        CurrentOwnerId = ownerId;
    }

    /// <summary>
    /// Changes current owner identifier.
    /// </summary>
    /// <param name="newOwnerId">
    /// New owner identifier.
    /// </param>
    public OwnerComponent ChangeOwnerId(EntityId newOwnerId) =>
        this with { CurrentOwnerId = newOwnerId };
}
