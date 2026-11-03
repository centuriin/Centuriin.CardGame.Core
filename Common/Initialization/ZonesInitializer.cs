using Centuriin.CardGame.Core.Common.Context;
using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Components.Players;
using Centuriin.CardGame.Core.Common.ECS.Components.Zones;
using Centuriin.CardGame.Core.Common.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Common.Storage.Repositories;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Initialization;

public sealed class ZonesInitializer : IGameEntityInitializer
{
    private readonly IZonesRepository _zonesRepository;
    private readonly IZoneFactory _zonesFactory;

    public ZonesInitializer(
        IZonesRepository zonesRepository,
        IZoneFactory zonesFactory)
    {
        ArgumentNullException.ThrowIfNull(zonesRepository);
        _zonesRepository = zonesRepository;

        ArgumentNullException.ThrowIfNull(zonesFactory);
        _zonesFactory = zonesFactory;
    }

    public async Task InitializeAsync(
        GameSetup setup,
        IInitialStateContext initialStateContext,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);
        ArgumentNullException.ThrowIfNull(initialStateContext);

        token.ThrowIfCancellationRequested();

        var zoneTemplateIds = await _zonesRepository.GetZoneTemplateIdsAsync(
            setup.GameTypeId,
            token);

        var zoneTemplates = await _zonesFactory.CreateAsync(
            zoneTemplateIds,
            setup.PlayerIds.Count(x => x != PlayerId.System),
            token);

        AddLinksBetweenPlayersAndZones(
            initialStateContext,
            zoneTemplates,
            PlayerRole.Participant,
            ZoneRole.Hand);

        AddLinksBetweenPlayersAndZones(
            initialStateContext,
            zoneTemplates,
            PlayerRole.Bank,
            ZoneRole.Deck);

        foreach (var zone in zoneTemplates)
        {
            initialStateContext.Entities.AddEntity(zone);
        }
    }

    private static void AddLinksBetweenPlayersAndZones(
        IInitialStateContext initialStateContext,
        IReadOnlyCollection<Zone> zones,
        PlayerRole playerRole,
        ZoneRole zoneRole)
    {
        var suitablePlayers = initialStateContext.Entities
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