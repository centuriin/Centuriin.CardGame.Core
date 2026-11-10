using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine;

public interface ITemplatesRepository<TTemplate>
    where TTemplate : TemplateBase
{
    public Task<TTemplate> GetByIdAsync(TemplateId templateId, CancellationToken token);

    public Task<IReadOnlyCollection<TTemplate>> GetTemplatesByIdsAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token);
}