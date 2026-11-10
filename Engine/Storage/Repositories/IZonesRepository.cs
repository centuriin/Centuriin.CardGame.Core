using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.Storage.Repositories;

public interface IZonesRepository
{
    public Task<IReadOnlyCollection<TemplateId>> GetZoneTemplateIdsAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}