using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common;

public interface ICardFactory
{
    public Task<Card> CreateAsync(TemplateId templateId, CardId instanceId, CancellationToken token);
}