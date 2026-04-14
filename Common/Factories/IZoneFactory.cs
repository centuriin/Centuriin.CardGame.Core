using Centuriin.CardGame.Core.Common.Entities.Zones;

namespace Centuriin.CardGame.Core.Common.Factories;

public interface IZoneFactory
{
    public Task<IReadOnlyCollection<Zone>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token);
}
