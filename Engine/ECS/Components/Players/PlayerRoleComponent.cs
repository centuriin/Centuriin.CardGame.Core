using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

namespace Centuriin.CardGame.Core.Engine.ECS.Components.Players;

/// <summary>
/// Assigns gameplay roles to a player entity.
/// </summary>
/// <param name="Role">
/// The gameplay role assigned to the entity.
/// </param>
public sealed record class PlayerRoleComponent(PlayerRole Role) : ComponentBase;