using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

/// <summary>
/// Creates zone entities from templates with sequential identifiers.
/// </summary>
public sealed class ZoneFactory : EntityFactoryBase<Zone, ZoneTemplate>
{
    /// <summary>
    /// Initializes a new <see cref="ZoneFactory"/> instance.
    /// </summary>
    /// <param name="entityIdProvider">
    /// The provider used to allocate entity identifiers.
    /// </param>
    public ZoneFactory(IEntityIdProvider entityIdProvider) 
        : base(entityIdProvider)
    {
    }

    /// <inheritdoc/>
    protected override Zone CreateEntity(EntityId id, ZoneTemplate template) => 
        new(id, [.. template.Components, new TemplateComponent(template.Id)]);
}
