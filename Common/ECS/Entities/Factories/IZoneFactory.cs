using Centuriin.CardGame.Core.Common.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Common.ECS.Templates;

namespace Centuriin.CardGame.Core.Common.ECS.Entities.Factories;

public interface IZoneFactory
{
    public Task<IReadOnlyCollection<Zone>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        int activePlayersCount,
        CancellationToken token);
}
