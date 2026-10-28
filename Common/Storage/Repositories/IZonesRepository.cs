using Centuriin.CardGame.Core.Common.ECS.Templates;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IZonesRepository
{
    public Task<IReadOnlyCollection<TemplateId>> GetZoneTemplateIdsAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}