using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

namespace Centuriin.CardGame.Core.Engine.ECS.Templates;

/// <summary>
/// Defines the components assigned to a newly created player.
/// </summary>
public sealed class PlayerTemplate : TemplateBase
{
    /// <summary>
    /// Initializes a new <see cref="PlayerTemplate"/> instance.
    /// </summary>
    /// <param name="id">
    /// The template identifier.
    /// </param>
    /// <param name="components">
    /// The components assigned to the entity or template.
    /// </param>
    public PlayerTemplate(
        TemplateId id,
        IReadOnlyCollection<ComponentBase> components)
        : base(id, components)
    {
    }

    /// <summary>
    /// Creates a template for a participant with the supplied external player identifier.
    /// </summary>
    /// <param name="id">
    /// The external player identifier.
    /// </param>
    /// <returns>
    /// A template containing the participant role and external player identifier.
    /// </returns>
    public static PlayerTemplate CreateDefault(PlayerId id) =>
        new(
            new(),
            [
                new PlayerRoleComponent(PlayerRole.Participant),
                new PlayerIdentifierComponent(id)
            ]);
}
