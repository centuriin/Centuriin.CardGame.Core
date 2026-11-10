using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public interface IZoneFactory
{
    public Task<IReadOnlyCollection<Zone>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        int activePlayersCount,
        CancellationToken token);
}
