using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface ICardTemplateRepository
{
    public Task<CardTemplate> GetByIdAsync(TemplateId templateId, CancellationToken token);

    public Task<IReadOnlyCollection<TemplateId>> GetTemplateIdsAsync(CancellationToken token);
}