using Centuriin.CardGame.Core.Common.ECS.Components;

namespace Centuriin.CardGame.Core.Common.ECS.Templates;

public sealed class CardTemplate : TemplateBase
{
    public CardTemplate(
        TemplateId id,
        IReadOnlyCollection<ComponentBase> components) : base(id, components)
    {
    }
}