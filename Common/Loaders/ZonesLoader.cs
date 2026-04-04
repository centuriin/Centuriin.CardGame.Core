using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Factories;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class ZonesLoader : IZonesLoader
{
    private readonly IGameState _gameState;
    private readonly IZonesFactory _zonesFactory;

    public ZonesLoader(
        IGameState gameState,
        IZonesFactory zonesFactory)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(zonesFactory);
        _zonesFactory = zonesFactory;
    }

    public async Task LoadAsync(GameTypeId gameTypeId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var zones = await _zonesFactory.CreateAsync(gameTypeId, token);

        AddLinksBeetwenPlayersAndZones(zones, PlayerRole.Participant, ZoneRole.Hand);
        AddLinksBeetwenPlayersAndZones(zones, PlayerRole.Bank, ZoneRole.Deck);

        foreach (var zone in zones)
        {
            _gameState.AddEntity<Zone, ZoneId>(zone);
        }
    }

    private void AddLinksBeetwenPlayersAndZones(
        IReadOnlyCollection<Zone> zones,
        PlayerRole playerRole, 
        ZoneRole zoneRole)
    {
        var suitablePlayers = _gameState
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
