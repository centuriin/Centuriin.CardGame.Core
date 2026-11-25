using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

/// <summary>
/// Creates card entities from templates with sequential identifiers and template links.
/// </summary>
public sealed class CardFactory : EntityFactoryBase<Card, CardTemplate>
{
    /// <summary>
    /// Initializes a new <see cref="CardFactory"/> instance.
    /// </summary>
    /// <param name="entityIdProvider">
    /// The provider used to allocate entity identifiers.
    /// </param>
    public CardFactory(IEntityIdProvider entityIdProvider) 
        : base(entityIdProvider)
    {
    }

    /// <inheritdoc/>
    protected override Card CreateEntity(EntityId id, CardTemplate template)
        => new(id, [.. template.Components, new TemplateComponent(template.Id)]);
}
