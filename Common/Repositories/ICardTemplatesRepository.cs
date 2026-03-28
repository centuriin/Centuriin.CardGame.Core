using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface ICardTemplatesRepository
{
    public Task<CardTemplate> GetByIdAsync(TemplateId templateId, CancellationToken token);

    public Task<IReadOnlyDictionary<TemplateId, CardTemplate>> GetTemplatesByIdsAsync(
        IReadOnlySet<TemplateId> templateIds,
        CancellationToken token);
}