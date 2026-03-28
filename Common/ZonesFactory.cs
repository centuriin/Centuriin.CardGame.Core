using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common;

public sealed class ZonesFactory : IZonesFactory
{
    private readonly IGameState _gameState;
    private readonly IZoneTemplatesRepository _templatesRepository;

    public ZonesFactory(
        IGameState gameState,
        IZoneTemplatesRepository templatesRepository)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(templatesRepository);
        _templatesRepository = templatesRepository;
    }

    public async Task CreateAsync(GameTypeId gameTypeId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var templates = await _templatesRepository
            .GetTemplatesByGameTypeAsync(gameTypeId, token);

        var index = 0;
        foreach (var template in templates)
        {
            var zone = CreateZone(template, ++index);

            _gameState.AddEntity<Zone, ZoneId>(zone);
        }
    }

    private static Zone CreateZone(ZoneTemplate template, int zoneId)
    {
        var zone = new Zone(new(zoneId));

        foreach (var component in template.Components)
        {
            zone.Add(component.Copy());
        }

        zone.Add(new TemplateComponent(template.Id));

        return zone;
    }
}
