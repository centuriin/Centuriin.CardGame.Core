using Centuriin.CardGame.Core.Common.Entities.Players;

namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Owner component.
/// </summary>
public sealed record class OwnerComponent : ComponentBase
{
    /// <summary>
    /// Current owner identifier.
    /// </summary>
    public PlayerId CurrentOwnerId { get; private set; }

    public OwnerComponent(PlayerId ownerId)
    {
        CurrentOwnerId = ownerId;
    }

    /// <summary>
    /// Changes current owner identifier.
    /// </summary>
    /// <param name="newOwnerId">
    /// New owner identifier.
    /// </param>
    public void ChangeOwnerId(PlayerId newOwnerId) => CurrentOwnerId = newOwnerId;
}
