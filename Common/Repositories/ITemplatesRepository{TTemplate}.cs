namespace Centuriin.CardGame.Core.Common.Repositories;

public interface ITemplatesRepository<TTemplate>
    where TTemplate : TemplateBase
{
    public Task<TTemplate> GetByIdAsync(TemplateId templateId, CancellationToken token);

    public Task<IReadOnlyCollection<TTemplate>> GetTemplatesByIdsAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token);
}