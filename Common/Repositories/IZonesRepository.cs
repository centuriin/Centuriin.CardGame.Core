namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IZonesRepository
{
    public Task<IReadOnlySet<TemplateId>> GetZoneTemplateIdsAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}