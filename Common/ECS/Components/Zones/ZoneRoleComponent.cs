using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Entities.Zones;

namespace Centuriin.CardGame.Core.Common.ECS.Components.Zones;

public sealed record class ZoneRoleComponent(ZoneRole Role) : ComponentBase;