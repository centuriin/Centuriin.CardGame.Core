using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public sealed class ZoneFactory : IZoneFactory
{
    public IReadOnlyCollection<Zone> Create(
        IReadOnlyCollection<ZoneTemplate> templates,
        int activePlayersCount)
    {
        var zones = new List<Zone>(templates.Count);

        var entityId = EntityId.Default;
        foreach (var template in templates)
        {
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

    private static Zone CreateZone(
        ZoneTemplate template,
        EntityId zoneId)
    {
        var zone = new Zone(zoneId);

        zone.Add(
            [..
                template.Components.Select(x => x.Copy()),
                new TemplateComponent(template.Id),
            ]);

        return zone;
    }
}
