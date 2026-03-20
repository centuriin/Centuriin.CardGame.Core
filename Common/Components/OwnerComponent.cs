namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Owner component.
/// </summary>
public sealed record class OwnerComponent : ComponentBase
{
    /// <summary>
    /// Current owner identifier.
    /// </summary>
    public OwnerId CurrentOwnerId { get; private set; }

    /// <summary>
    /// Changes current owner identifier.
    /// </summary>
    /// <param name="newOwnerId">
    /// New owner identifier.
    /// </param>
    public void ChangeOwnerId(OwnerId newOwnerId) => CurrentOwnerId = newOwnerId;
}
