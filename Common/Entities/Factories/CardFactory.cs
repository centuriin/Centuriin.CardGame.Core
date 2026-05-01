using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Templates;

namespace Centuriin.CardGame.Core.Common.Entities.Factories;

public sealed class CardFactory : ICardFactory
{
    private readonly ITemplatesRepository<CardTemplate> _templatesRepository;

    public CardFactory(ITemplatesRepository<CardTemplate> templatesRepository)
    {
        ArgumentNullException.ThrowIfNull(templatesRepository);
        _templatesRepository = templatesRepository;
    }

    public async Task<IReadOnlyCollection<Card>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var templates = (await _templatesRepository.GetTemplatesByIdsAsync(templateIds, token))
            .ToDictionary(k => k.Id);

        var cards = new List<Card>(templateIds.Count);

        var entityId = EntityId.Default;
        foreach (var templateId in templateIds)
        {
            var template = templates[templateId];

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
