using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;

namespace Centuriin.CardGame.Core.Engine.ECS.Components.Zones;

/// <summary>
/// Assigns a gameplay role to a zone.
/// </summary>
/// <param name="Role">
/// The gameplay role assigned to the entity.
/// </param>
public sealed record class ZoneRoleComponent(ZoneRole Role) : ComponentBase;