namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Space component.
/// </summary>
public sealed record class SpaceComponent : ComponentBase
{
    /// <summary>
    /// Current space id.
    /// </summary>
    public SpaceId CurrentSpaceId { get; private set; }

    /// <summary>
    /// Creates new instance of <see cref="SpaceComponent"/>.
    /// </summary>
    /// <param name="spaceId">
    /// Space id.
    /// </param>
    public SpaceComponent(SpaceId spaceId)
    {
        CurrentSpaceId = spaceId;
    }

    /// <summary>
    /// Changes space id.
    /// </summary>
    /// <param name="newSpaceId">
    /// New space id.
    /// </param>
    public void ChangeSpaceId(SpaceId newSpaceId) => CurrentSpaceId = newSpaceId;
}