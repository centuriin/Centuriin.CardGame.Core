using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common;

public sealed class ZonesFactory : IZonesFactory
{
    private readonly IGameState _gameState;
    private readonly IZoneTemplatesRepository _zoneTemplatesRepository;

    public ZonesFactory(
        IGameState gameState,
        IZoneTemplatesRepository zoneTemplatesRepository)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(zoneTemplatesRepository);
        _zoneTemplatesRepository = zoneTemplatesRepository;
    }

    public async Task CreateAsync(GameTypeId gameTypeId, CancellationToken token)
    {
        var templatesIds = await _zoneTemplatesRepository
            .GetTemplateIdsByGameTypeAsync(gameTypeId, token);


        var index = 1;
        foreach (var templateId in templatesIds)
        {
            var zone = await CreateAsync(templateId, new(index), token);

            _gameState.AddEntity(zone);
        }
    }

    private async Task<Zone> CreateAsync(
        TemplateId templateId, 
        ZoneId zoneId, 
        CancellationToken token)
    {
        var zone = new Zone(zoneId);

        var template = await _zoneTemplatesRepository.GetByIdAsync(templateId, token);

        foreach (var component in template.Components)
        {
            zone.Add(component.Copy());
        }

        zone.Add(new TemplateComponent(templateId));

        return zone;
    }
}
