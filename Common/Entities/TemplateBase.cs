using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities;

public abstract class TemplateBase
{
    public TemplateId Id { get; }

    public IReadOnlyCollection<ComponentBase> Components { get; }

    protected TemplateBase(TemplateId id, IReadOnlyCollection<ComponentBase> components)
    {
        Id = id;
        Components = components;
    }
}
