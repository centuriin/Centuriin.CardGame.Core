using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities.Zones;

public sealed class ZoneTemplate : TemplateBase
{
    public ZoneTemplate(
        TemplateId id,
        IReadOnlyCollection<IComponent> components) : base(id, components)
    {
    }
}
