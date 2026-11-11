using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public sealed class CardFactory : ICardFactory
{
    public IReadOnlyCollection<Card> Create(
        IReadOnlyCollection<CardTemplate> templates)
    {
        var cards = new List<Card>(templates.Count);

        var entityId = EntityId.Default;
        foreach (var template in templates)
        {
            cards.Add(CreateCard(template, ++entityId));
        }

        return cards;
    }

    private static Card CreateCard(CardTemplate template, EntityId cardId)
    {
        var card = new Card(cardId);

        card.Add(
            [..
                template.Components.Select(x => x.Copy()),
                new TemplateComponent(template.Id)
            ]);

        return card;
    }
}
