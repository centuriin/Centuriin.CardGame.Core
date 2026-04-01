using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common;

public sealed class ClassicDealerService : IClassicDealerService
{
    private readonly IGame _game;

    public ClassicDealerService(IGame game)
    {
        ArgumentNullException.ThrowIfNull(game);
        _game = game;
    }

    public async Task DealCardsAsync(int count, CancellationToken token)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(count, 0);

        token.ThrowIfCancellationRequested();

        var generalDeck = _game.State
            .Query<Card>()
            .WithComponent<OwnerComponent>(x => x.CurrentOwnerId == PlayerId.System)
            .As<Card>()
            .Select(x => x.Id)
            .Shuffle()
            .Take(count);

        var pickQueue = new Queue<CardId>(generalDeck);

        foreach (var player in _game.State.Query<Player>())
        {
            if (player.Id == PlayerId.System)
                continue;

            var pickedCardId = pickQueue.Dequeue();

            var @event = new CardDealtEvent(_game.State.GameId, pickedCardId, player.Id);

            await _game.ApplyAsync(@event, token);
        }

        await _game.ApplyAsync(new GameStartedEvent(_game.State.GameId), token);
    }
}