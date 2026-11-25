using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.ECS.Components;

/// <summary>
/// Identifies the zone containing a card.
/// </summary>
/// <param name="CurrentZoneId">
/// Current zone id.
/// </param>
public sealed record class ZoneComponent(EntityId CurrentZoneId) : ComponentBase;
