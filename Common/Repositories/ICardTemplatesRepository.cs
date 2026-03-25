using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface ICardTemplatesRepository
{
    public Task<CardTemplate> GetByIdAsync(TemplateId templateId, CancellationToken token);

    public Task<IReadOnlyCollection<TemplateId>> GetTemplateIdsByGameTypeAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}