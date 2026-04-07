namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IZonesRepository
{
    public Task<IReadOnlyCollection<TemplateId>> GetZoneTemplateIdsAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}