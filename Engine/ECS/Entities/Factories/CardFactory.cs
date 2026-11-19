using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public sealed class CardFactory : EntityFactoryBase<Card, CardTemplate>
{
    public CardFactory(IEntityIdProvider entityIdProvider) 
        : base(entityIdProvider)
    {
    }

    protected override Card CreateEntity(EntityId id, CardTemplate template)
        => new(id, [.. template.Components, new TemplateComponent(template.Id)]);
}
