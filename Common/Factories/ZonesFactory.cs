using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common.Factories;

public sealed class ZonesFactory : IZonesFactory
{
    private readonly ITemplatesRepository<ZoneTemplate> _templatesRepository;

    public ZonesFactory(ITemplatesRepository<ZoneTemplate> templatesRepository)
    {
        ArgumentNullException.ThrowIfNull(templatesRepository);
        _templatesRepository = templatesRepository;
    }

    public async Task<IReadOnlyCollection<Zone>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var templates = (await _templatesRepository.GetTemplatesByIdsAsync(templateIds, token))
            .ToDictionary(k => k.Id);

        var zones = new List<Zone>(templateIds.Count);

        var index = 0;
        foreach (var templateId in templateIds)
        {
            var template = templates[templateId];

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
