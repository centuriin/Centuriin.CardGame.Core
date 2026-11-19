using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public sealed class ZoneFactory : EntityFactoryBase<Zone, ZoneTemplate>
{
    public ZoneFactory(IEntityIdProvider entityIdProvider) 
        : base(entityIdProvider)
    {
    }

    protected override Zone CreateEntity(EntityId id, ZoneTemplate template) => 
        new(id, [.. template.Components, new TemplateComponent(template.Id)]);
}
