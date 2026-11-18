using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public sealed class CardFactory : ICardFactory
{
    public IReadOnlyCollection<EditableEntity<Card>> Create(
        IReadOnlyCollection<CardTemplate> templates)
    {
        var cards = new List<EditableEntity<Card>>(templates.Count);

        var entityId = EntityId.Default;
        foreach (var template in templates)
        {
            cards.Add(
                new(new Card(
                    ++entityId,
                    [.. template.Components, new TemplateComponent(template.Id)])));
        }

        return cards;
    }
}
