using Centuriin.CardGame.Core.Common.Cards;

namespace Centuriin.CardGame.Core.Common;

public interface ICardFactory
{
    Task<Card> CreateAsync(TemplateId templateId, CardId instanceId, CancellationToken token);
}