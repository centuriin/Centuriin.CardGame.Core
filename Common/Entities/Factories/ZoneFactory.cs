using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Templates;

namespace Centuriin.CardGame.Core.Common.Entities.Factories;

public sealed class ZoneFactory : IZoneFactory
{
    private readonly ITemplatesRepository<ZoneTemplate> _templatesRepository;

    public ZoneFactory(ITemplatesRepository<ZoneTemplate> templatesRepository)
    {
        ArgumentNullException.ThrowIfNull(templatesRepository);
        _templatesRepository = templatesRepository;
    }

    public async Task<IReadOnlyCollection<Zone>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        int activePlayersCount,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var templates = (await _templatesRepository.GetTemplatesByIdsAsync(templateIds, token))
            .ToDictionary(k => k.Id);

        var zones = new List<Zone>(templateIds.Count);

        var entityId = EntityId.Default;
        foreach (var templateId in templateIds)
        {
            var template = templates[templateId];

            if (template.Scope is ZoneScope.Singleton)
            {
                zones.Add(CreateZone(template, ++entityId));
            }
            else if (template.Scope is ZoneScope.PerPlayer)
            {
                for (var i = 0; i < activePlayersCount; i++)
                {
                    zones.Add(CreateZone(template, ++entityId));
                }
            }
        }

        return zones;
    }

    private static Zone CreateZone(ZoneTemplate template, EntityId zoneId)
    {
        var zone = new Zone(zoneId);

        zone.Add(
            [..
                template.Components.Select(x => x.Copy()),
                new TemplateComponent(template.Id)
            ]);

        return zone;
    }
}
