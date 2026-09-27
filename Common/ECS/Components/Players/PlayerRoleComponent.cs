using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;

namespace Centuriin.CardGame.Core.Common.ECS.Components.Players;

public sealed record class PlayerRoleComponent(PlayerRole Role) : ComponentBase;