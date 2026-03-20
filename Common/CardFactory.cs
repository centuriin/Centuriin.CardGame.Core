using System;
using System.Collections.Generic;
using System.Text;

using Centuriin.CardGame.Core.Common.Cards;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common;

public sealed class CardFactory
{
    private readonly ICardTemplateRepository _repository;

    public CardFactory(ICardTemplateRepository repository)
    {
        _repository = repository;
    }

    public static Card Create(TemplateId templateId, int instanceId)
    {
        //var template = _repository.GetById(templateId);
        var template = DefaultCardTemplatesRepository.Templates36[templateId];

        var card = new Card(new CardId(instanceId));

        foreach (var component in template.Components)
        {
            // Используем твой метод Copy/Clone
            card.Add(component.Copy());
        }

        return card;
    }
}
