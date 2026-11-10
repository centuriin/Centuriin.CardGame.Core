using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

namespace Centuriin.CardGame.Core.Engine.ECS.Components.Players;

public sealed record class PlayerRoleComponent(PlayerRole Role) : ComponentBase;