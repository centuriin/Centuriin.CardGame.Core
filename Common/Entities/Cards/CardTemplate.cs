using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities.Cards;

public sealed class CardTemplate : TemplateBase
{
    public CardTemplate(
        TemplateId id, 
        IReadOnlyCollection<IComponent> components) : base(id, components)
    {
    }
}