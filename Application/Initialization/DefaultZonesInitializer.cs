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

/// <summary>
/// Creates zones from templates and assigns hand and deck owners.
/// </summary>
public sealed class DefaultZonesInitializer : IEntityInitializer
{
    private readonly ITemplatesRepository<ZoneTemplate> _zoneTemplatesRepository;
    private readonly IEntityFactory<Zone, ZoneTemplate> _factory;

    /// <summary>
    /// Initializes a new <see cref="DefaultZonesInitializer"/> instance.
    /// </summary>
    /// <param name="zoneTemplatesRepository">
    /// The repository used to retrieve zone templates.
    /// </param>
    /// <param name="factory">
    /// The factory used to create the required entities or rules.
    /// </param>
    public DefaultZonesInitializer(
        ITemplatesRepository<ZoneTemplate> zoneTemplatesRepository,
        IEntityFactory<Zone, ZoneTemplate> factory)
    {
        ArgumentNullException.ThrowIfNull(zoneTemplatesRepository);
        _zoneTemplatesRepository = zoneTemplatesRepository;

        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<EntityBase>> InitializeAsync(
        GameDescription description,
        IReadOnlyInitialStateContext context,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(context);

        token.ThrowIfCancellationRequested();

        var zoneTemplates = await _zoneTemplatesRepository.GetTemplatesAsync(
            description.GameTypeId,
            token);

        var activePlayersCount = description.PlayerIds
            .Count(x => x != PlayerId.System);

        var templates = zoneTemplates
            .Select(x =>
                (x.Scope) switch
                {
                    ZoneScope.PerPlayer => Enumerable.Repeat(x, activePlayersCount),
                    _ => [x]
                })
            .SelectMany(x => x)
            .ToList();

        var zones = _factory.Create(templates);

        AddLinksBetweenPlayersAndZones(
            context,
            zones,
            PlayerRole.Participant,
            ZoneRole.Hand);

        AddLinksBetweenPlayersAndZones(
            context,
            zones,
            PlayerRole.Bank,
            ZoneRole.Deck);

        return [.. zones.Select(x => x.Entity)];
    }

    private static void AddLinksBetweenPlayersAndZones(
        IReadOnlyInitialStateContext context,
        IReadOnlyCollection<EditableEntity<Zone>> zones,
        PlayerRole playerRole,
        ZoneRole zoneRole)
    {
        var suitablePlayers = context.Entities
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