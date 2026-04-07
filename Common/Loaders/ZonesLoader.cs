using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class ZonesLoader : IGameLoader
{
    private readonly IZoneDefinitionsRepository _zoneDefinitionsRepository;
    private readonly IZonesFactory _zonesFactory;

    public ZonesLoader(
        IZoneDefinitionsRepository zonesRepository,
        IZonesFactory zonesFactory)
    {
        ArgumentNullException.ThrowIfNull(zonesRepository);
        _zoneDefinitionsRepository = zonesRepository;

        ArgumentNullException.ThrowIfNull(zonesFactory);
        _zonesFactory = zonesFactory;
    }

    public async Task LoadAsync(GameSetup setup, IGameState gameState, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);
        ArgumentNullException.ThrowIfNull(gameState);

        token.ThrowIfCancellationRequested();

        var zoneDefinitions = await _zoneDefinitionsRepository.GetZoneDefinitionsAsync(
            setup.GameTypeId,
            token);

        var zoneTemplates = await CreateZonesAsync(
            setup.PlayerIds.Count,
            zoneDefinitions,
            token);

        AddLinksBeetwenPlayersAndZones(
            gameState,
            zoneTemplates,
            PlayerRole.Participant,
            ZoneRole.Hand);

        AddLinksBeetwenPlayersAndZones(
            gameState,
            zoneTemplates,
            PlayerRole.Bank,
            ZoneRole.Deck);

        foreach (var zone in zoneTemplates)
        {
            gameState.AddEntity<Zone, ZoneId>(zone);
        }
    }

    private async Task<IReadOnlyCollection<Zone>> CreateZonesAsync(
        int playersCount,
        IReadOnlyCollection<ZoneDefinition> zoneDefinitions,
        CancellationToken token)
    {
        var zoneTemplateIds = new List<TemplateId>();

        foreach (var definition in zoneDefinitions)
        {
            if (definition.Scope == ZoneScope.Singleton)
            {
                zoneTemplateIds.Add(definition.TemplateId);
            }
            else if (definition.Scope == ZoneScope.PerPlayer)
            {
                zoneTemplateIds.AddRange(Enumerable.Repeat(definition.TemplateId, playersCount));
            }
        }

        return await _zonesFactory.CreateAsync(zoneTemplateIds, token);
    }

    private static void AddLinksBeetwenPlayersAndZones(
        IGameState gameState,
        IReadOnlyCollection<Zone> zones,
        PlayerRole playerRole,
        ZoneRole zoneRole)
    {
        var suitablePlayers = gameState
            .Query<Player>()
            .WithComponent<PlayerRoleComponent>(x => x.Role.HasFlag(playerRole));

        var suitableZones = zones
            .WithComponent<ZoneRoleComponent>(x => x.Role == zoneRole);

        foreach (var (zone, player) in suitableZones.Zip(suitablePlayers))
        {
            var ownerId = ((Player)player).Id;

            zone.Add(new OwnerComponent(ownerId));
        }
    }
}