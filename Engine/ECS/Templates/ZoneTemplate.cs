using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Templates;

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
