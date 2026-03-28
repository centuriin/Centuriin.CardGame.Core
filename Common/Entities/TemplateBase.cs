using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities;

public abstract class TemplateBase
{
    public TemplateId Id { get; }

    public IReadOnlyCollection<IGameComponent> Components { get; }

    protected TemplateBase(TemplateId id, IReadOnlyCollection<IGameComponent> components)
    {
        Id = id;
        Components = components;
    }
}
