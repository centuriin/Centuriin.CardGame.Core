using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Templates;

public sealed class ZoneTemplate : TemplateBase
{
    public ZoneScope Scope { get; }

    public ZoneTemplate(
        TemplateId id,
        IReadOnlyCollection<ComponentBase> components,
        ZoneScope scope) : base(id, components)
    {
        Scope = scope;
    }
}
