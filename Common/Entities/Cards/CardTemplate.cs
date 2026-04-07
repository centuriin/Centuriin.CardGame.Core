using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities.Cards;

public sealed class CardTemplate : TemplateBase
{
    public CardTemplate(
        TemplateId id,
        IReadOnlyCollection<ComponentBase> components) : base(id, components)
    {
    }
}