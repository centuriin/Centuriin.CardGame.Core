using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Components.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Templates;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application.Initialization;

public sealed class ZonesInitializer : IGameEntityInitializer
{
    private readonly ITemplatesRepository<ZoneTemplate> _zoneTemplatesRepository;
    private readonly IZoneFactory _zonesFactory;

    public ZonesInitializer(
        ITemplatesRepository<ZoneTemplate> zoneTemplatesRepository,
        IZoneFactory zonesFactory)
    {
        ArgumentNullException.ThrowIfNull(zoneTemplatesRepository);
        _zoneTemplatesRepository = zoneTemplatesRepository;

        ArgumentNullException.ThrowIfNull(zonesFactory);
        _zonesFactory = zonesFactory;
    }

    public async Task InitializeAsync(
        GameDescription gameDescription,
        IInitialStateContext initialStateContext,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(gameDescription);
        ArgumentNullException.ThrowIfNull(initialStateContext);

        token.ThrowIfCancellationRequested();

        var zoneTemplateIds = await _zoneTemplatesRepository.GetTemplatesAsync(
            gameDescription.GameTypeId,
            token);

        var zones = _zonesFactory.Create(
            zoneTemplateIds,
            gameDescription.PlayerIds.Count(x => x != PlayerId.System));

        AddLinksBetweenPlayersAndZones(
            initialStateContext,
            zones,
            PlayerRole.Participant,
            ZoneRole.Hand);

        AddLinksBetweenPlayersAndZones(
            initialStateContext,
            zones,
            PlayerRole.Bank,
            ZoneRole.Deck);

        foreach (var zone in zones)
        {
            initialStateContext.Entities.Add(zone);
        }
    }

    private static void AddLinksBetweenPlayersAndZones(
        IInitialStateContext initialStateContext,
        IReadOnlyCollection<EditableEntity<Zone>> zones,
        PlayerRole playerRole,
        ZoneRole zoneRole)
    {
        var suitablePlayers = initialStateContext.Entities
            .Query<Player>()
            .WithComponent((PlayerRoleComponent x) => x.Role.HasFlag(playerRole));

        var suitableZones = zones
            .WithComponent((ZoneRoleComponent x) => x.Role == zoneRole);

        foreach (var (zone, player) in suitableZones.Zip(suitablePlayers))
        {
            var ownerId = player.Id;

            zone.Set(new OwnerComponent(ownerId));
        }
    }
}