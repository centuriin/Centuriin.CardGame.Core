using Centuriin.CardGame.Core.Common.Entities.Zones;

namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Space component.
/// </summary>
public sealed record class ZoneComponent : ComponentBase
{
    /// <summary>
    /// Current space id.
    /// </summary>
    public ZoneId CurrentZoneId { get; private set; }

    /// <summary>
    /// Creates new instance of <see cref="ZoneComponent"/>.
    /// </summary>
    /// <param name="spaceId">
    /// Space id.
    /// </param>
    public ZoneComponent(ZoneId spaceId)
    {
        CurrentZoneId = spaceId;
    }

    /// <summary>
    /// Changes space id.
    /// </summary>
    /// <param name="newSpaceId">
    /// New space id.
    /// </param>
    public void ChangeZoneId(ZoneId newSpaceId) => CurrentZoneId = newSpaceId;
}