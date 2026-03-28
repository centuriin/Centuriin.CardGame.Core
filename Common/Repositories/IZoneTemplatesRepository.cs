using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Zones;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IZoneTemplatesRepository
{
    public Task<ZoneTemplate> GetByIdAsync(TemplateId templateId, CancellationToken token);

    public Task<IReadOnlyCollection<ZoneTemplate>> GetTemplatesByGameTypeAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}
