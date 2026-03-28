using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;

namespace Centuriin.CardGame.Core.Common;

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

        AddOwnerComponents(zones);

        foreach (var zone in zones)
        {
            _gameState.AddEntity<Zone, ZoneId>(zone);
        }
    }

    private void AddOwnerComponents(IReadOnlyCollection<Zone> zones)
    {
        var paticipantPlayers = _gameState
            .Query<Player>()
            .WithComponent<PlayerRoleComponent>(x => x.Role == PlayerRole.Participant);

        var handZones = zones.WithComponent<ZoneRoleComponent>(x => x.Role == ZoneRole.Hand);

        foreach (var (zone, player) in handZones.Zip(paticipantPlayers))
        {
            var ownerId = new OwnerId(((Player)player).Id.Value);

            zone.Add(new OwnerComponent(ownerId));
        }

        var playersWithDeck = _gameState
            .Query<Player>()
            .WithComponent<HasDeckComponent>(x => x.HasDeck);

        var deckZones = zones.WithComponent<ZoneRoleComponent>(x => x.Role == ZoneRole.Deck);

        foreach (var (zone, player) in deckZones.Zip(playersWithDeck))
        {
            var ownerId = new OwnerId(((Player)player).Id.Value);

            zone.Add(new OwnerComponent(ownerId));
        }
    }
}
