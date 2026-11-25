using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Templates;

/// <summary>
/// Defines a zone's initial components and creation scope.
/// </summary>
public sealed class ZoneTemplate : TemplateBase
{
    /// <summary>
    /// Gets the number and ownership scope of zones created from this template.
    /// </summary>
    public ZoneScope Scope { get; }

    /// <summary>
    /// Initializes a new <see cref="ZoneTemplate"/> instance.
    /// </summary>
    /// <param name="id">
    /// The template identifier.
    /// </param>
    /// <param name="components">
    /// The components assigned to the entity or template.
    /// </param>
    /// <param name="scope">
    /// The dependency injection scope owned by the session.
    /// </param>
    public ZoneTemplate(
        TemplateId id,
        IReadOnlyCollection<ComponentBase> components,
        ZoneScope scope) : base(id, components)
    {
        Scope = scope;
    }
}
