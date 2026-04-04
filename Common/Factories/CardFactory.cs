using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common.Factories;

public sealed class CardsFactory : ICardsFactory
{
    private readonly ICardTemplatesRepository _templatesRepository;

    public CardsFactory(ICardTemplatesRepository templatesRepository)
    {
        ArgumentNullException.ThrowIfNull(templatesRepository);
        _templatesRepository = templatesRepository;
    }

    public async Task<IReadOnlyCollection<Card>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var uniqueIds = templateIds.ToHashSet();

        var templates = await _templatesRepository.GetTemplatesByIdsAsync(uniqueIds, token);

        var cards = new List<Card>(templateIds.Count);

        var index = 0;
        foreach (var templateId in templateIds)
        {
            var template = templates[templateId];

            cards.Add(CreateCard(template, ++index));
        }

        return cards;
    }

    private static Card CreateCard(CardTemplate template, int cardId)
    {
        var card = new Card(new CardId(cardId));

        card.Add(
            [.. 
                template.Components.Select(x => x.Copy()), 
                new TemplateComponent(template.Id)
            ]);

        return card;
    }
}
