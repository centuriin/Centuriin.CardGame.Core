using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities;

public abstract class TemplateBase
{
    public TemplateId Id { get; }

    public IReadOnlyCollection<IComponent> Components { get; }

    protected TemplateBase(TemplateId id, IReadOnlyCollection<IComponent> components)
    {
        Id = id;
        Components = components;
    }
}
