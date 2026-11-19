using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

namespace Centuriin.CardGame.Core.Engine.ECS.Templates;

public sealed class PlayerTemplate : TemplateBase
{
    public PlayerTemplate(
        TemplateId id,
        IReadOnlyCollection<ComponentBase> components)
        : base(id, components)
    {
    }

    public static PlayerTemplate CreateDefault(PlayerId id) =>
        new(
            new(),
            [
                new PlayerRoleComponent(PlayerRole.Participant),
                new PlayerIdentifierComponent(id)
            ]);
}
