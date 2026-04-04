using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common.Factories;

public sealed class ZonesFactory : IZonesFactory
{
    private readonly IZoneTemplatesRepository _templatesRepository;

    public ZonesFactory(IZoneTemplatesRepository templatesRepository)
    {
        ArgumentNullException.ThrowIfNull(templatesRepository);
        _templatesRepository = templatesRepository;
    }

    public async Task<IReadOnlyCollection<Zone>> CreateAsync(
        GameTypeId gameTypeId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var templates = await _templatesRepository
            .GetTemplatesByGameTypeAsync(gameTypeId, token);

        var zones = new List<Zone>(templates.Count);

        var index = 0;
        foreach (var template in templates)
        {
            zones.Add(CreateZone(template, ++index));
        }

        return zones;
    }

    private static Zone CreateZone(ZoneTemplate template, int zoneId)
    {
        var zone = new Zone(new(zoneId));

        zone.Add(
            [..
                template.Components.Select(x => x.Copy()),
                new TemplateComponent(template.Id)
            ]);

        return zone;
    }
}
