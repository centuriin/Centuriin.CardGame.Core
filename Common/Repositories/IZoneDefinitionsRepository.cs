namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IZoneDefinitionsRepository
{
    public Task<IReadOnlyCollection<ZoneDefinition>> GetZoneDefinitionsAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}