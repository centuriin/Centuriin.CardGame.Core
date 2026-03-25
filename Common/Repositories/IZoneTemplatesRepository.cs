using Centuriin.CardGame.Core.Common.Entities.Zones;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IZoneTemplatesRepository
{
    public Task<ZoneTemplate> GetByIdAsync(TemplateId templateId, CancellationToken token);

    public Task<IReadOnlyCollection<TemplateId>> GetTemplateIdsByGameTypeAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}
