using Centuriin.CardGame.Core.Common.Entities;

namespace Centuriin.CardGame.Core.Common.Components;

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
    public void ChangeOwnerId(EntityId newOwnerId) => CurrentOwnerId = newOwnerId;
}
