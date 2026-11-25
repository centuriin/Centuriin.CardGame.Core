using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Templates;

/// <summary>
/// Defines the components assigned to a newly created card.
/// </summary>
public sealed class CardTemplate : TemplateBase
{
    /// <summary>
    /// Initializes a new <see cref="CardTemplate"/> instance.
    /// </summary>
    /// <param name="id">
    /// The template identifier.
    /// </param>
    /// <param name="components">
    /// The components assigned to the entity or template.
    /// </param>
    public CardTemplate(
        TemplateId id,
        IReadOnlyCollection<ComponentBase> components) : base(id, components)
    {
    }
}