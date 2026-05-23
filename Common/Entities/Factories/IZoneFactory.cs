using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Templates;

namespace Centuriin.CardGame.Core.Common.Entities.Factories;

public interface IZoneFactory
{
    public Task<IReadOnlyCollection<Zone>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        int activePlayersCount,
        CancellationToken token);
}
