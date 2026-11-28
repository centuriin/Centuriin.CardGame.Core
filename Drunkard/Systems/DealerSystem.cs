using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Components.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Drunkard.Systems;

/// <summary>
/// Reserves the Drunkard-specific dealing system implementation.
/// </summary>
internal sealed class DealerSystem :
    ISystem,
    IProducingSubscriber<GameStartedEvent>
{
    private const int PLAYERS_COUNT = 2;

    public void OnEvent(
        GameStartedEvent @event,
        IReadOnlyGameState readOnlyGameState,
        IGameEventBusWriter eventBusWriter)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(readOnlyGameState);
        ArgumentNullException.ThrowIfNull(eventBusWriter);

        var deckId = readOnlyGameState.Entities.Query<Zone>()
            .WithComponent((ZoneRoleComponent x) => x.Role is ZoneRole.Deck)
            .Single()
            .Id;

        var deckCards = readOnlyGameState.Entities.Query<Card>()
            .WithComponent((ZoneComponent x) => x.CurrentZoneId == deckId)
            .Shuffle()
            .ToList();

        var activePlayers = readOnlyGameState.Entities.Query<Player>()
            .WithComponent((PlayerRoleComponent x) => x.Role.HasFlag(PlayerRole.Participant))
            .ToList();

        if (activePlayers.Count != PLAYERS_COUNT)
        {
            throw new InvalidOperationException(
                $"Active players count should be {PLAYERS_COUNT}," +
                $" but found {activePlayers.Count}");
        }

        var cardPerPlayerCount = deckCards.Count / PLAYERS_COUNT;

        for (var i = 0; i < activePlayers.Count; i++)
        {
            var player = activePlayers[i];

            var hand = readOnlyGameState.Entities.Query<Zone>()
                .WithComponent((OwnerComponent x) => x.CurrentOwnerId == player.Id)
                .Single();

            foreach (var card in deckCards
                .Skip(i * cardPerPlayerCount)
                .Take(cardPerPlayerCount))
            {
                eventBusWriter.Write(new CardDealtEvent(@event.GameId, card.Id, hand.Id));
            }
        }
    }
}
