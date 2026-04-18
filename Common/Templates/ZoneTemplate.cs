using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Templates;

public sealed class ZoneTemplate : TemplateBase
{
    public ZoneTemplate(
        TemplateId id,
        IReadOnlyCollection<ComponentBase> components) : base(id, components)
    {
    }
}
