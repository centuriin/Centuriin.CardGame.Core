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
    private readonly IZonesRepository _zonesRepository;
    private readonly IZonesFactory _zonesFactory;

    public ZonesLoader(
        IZonesRepository zonesRepository,
        IZonesFactory zonesFactory)
    {
        ArgumentNullException.ThrowIfNull(zonesRepository);
        _zonesRepository = zonesRepository;

        ArgumentNullException.ThrowIfNull(zonesFactory);
        _zonesFactory = zonesFactory;
    }

    public async Task LoadAsync(GameSetup setup, IGameState gameState, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);
        ArgumentNullException.ThrowIfNull(gameState);

        token.ThrowIfCancellationRequested();

        var zoneTemplateIds = await _zonesRepository.GetZoneTemplateIdsAsync(setup.GameTypeId, token);

        var zoneTemplates = await _zonesFactory.CreateAsync(zoneTemplateIds, token);

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