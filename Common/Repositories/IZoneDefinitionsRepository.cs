using Centuriin.CardGame.Core.Common.Templates;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IZoneDefinitionsRepository
{
    public Task<IReadOnlyCollection<ZoneDefinition>> GetZoneDefinitionsAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}