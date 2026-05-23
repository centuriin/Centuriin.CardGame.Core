using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Factories;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Templates;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class ZonesLoader : IGameLoader
{
    private readonly IZonesRepository _zonesRepository;
    private readonly IZoneFactory _zonesFactory;

    public ZonesLoader(
        IZonesRepository zonesRepository,
        IZoneFactory zonesFactory)
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

        var zoneTemplateIds = await _zonesRepository.GetZoneTemplateIdsAsync(
            setup.GameTypeId,
            token);

        var zoneTemplates = await _zonesFactory.CreateAsync(
            zoneTemplateIds,
            setup.PlayerIds.Count(x => x != PlayerId.System),
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
            gameState.AddEntity(zone);
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