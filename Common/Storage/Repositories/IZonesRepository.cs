using Centuriin.CardGame.Core.Common.ECS.Templates;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IZonesRepository
{
    public Task<IReadOnlyCollection<TemplateId>> GetZoneTemplateIdsAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}