using Centuriin.CardGame.Core.Common.Entities.Zones;

namespace Centuriin.CardGame.Core.Common.Factories;

public interface IZonesFactory
{
    public Task<IReadOnlyCollection<Zone>> CreateAsync(GameTypeId gameTypeId, CancellationToken token);
}
