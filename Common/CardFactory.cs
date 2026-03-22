using Centuriin.CardGame.Core.Common.Cards;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common;

public sealed class CardFactory
{
    private readonly ICardTemplateRepository _repository;

    public CardFactory(ICardTemplateRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    public async Task<Card> CreateAsync(TemplateId templateId, CardId instanceId, CancellationToken token)
    {
        var template = await _repository.GetByIdAsync(templateId, token);

        var card = new Card(instanceId);

        foreach (var component in template.Components)
        {
            card.Add(component.Copy());
        }

        return card;
    }
}
