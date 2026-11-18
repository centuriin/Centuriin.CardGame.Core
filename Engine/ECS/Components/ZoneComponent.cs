using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.ECS.Components;

/// <summary>
/// Space component.
/// </summary>
/// <param name="CurrentZoneId">
/// Current zone id.
/// </param>
public sealed record class ZoneComponent(EntityId CurrentZoneId) : ComponentBase;