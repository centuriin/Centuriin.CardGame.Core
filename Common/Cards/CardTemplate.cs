using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Cards;

public sealed class CardTemplate
{
    public TemplateId Id { get; }

    public IReadOnlyCollection<IComponent> Components { get; }

    public CardTemplate(TemplateId id, IReadOnlyCollection<IComponent> components)
    {
        Id = id;
        Components = components;
    }
}