using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Templates;

public sealed class CardTemplate : TemplateBase
{
    public CardTemplate(
        TemplateId id,
        IReadOnlyCollection<ComponentBase> components) : base(id, components)
    {
    }
}