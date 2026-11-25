using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Templates;

/// <summary>
/// Stores a template identifier and the components used to create an entity.
/// </summary>
public abstract class TemplateBase
{
    /// <summary>
    /// Gets the identifier of this entity template.
    /// </summary>
    public TemplateId Id { get; }

    /// <summary>
    /// Gets the components assigned to entities created from this template.
    /// </summary>
    public IReadOnlyCollection<ComponentBase> Components { get; }

    /// <summary>
    /// Initializes a new <see cref="TemplateBase"/> instance.
    /// </summary>
    /// <param name="id">
    /// The template identifier.
    /// </param>
    /// <param name="components">
    /// The components assigned to the entity or template.
    /// </param>
    protected TemplateBase(TemplateId id, IReadOnlyCollection<ComponentBase> components)
    {
        Id = id;
        Components = components;
    }
}
