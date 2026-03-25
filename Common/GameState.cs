using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.System;
using Centuriin.Centuriin.Core.Common;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameState
{
    private readonly GameId _gameId;
    private readonly Dictionary<CardId, Card> _cards = [];

    private readonly IEventDispatcher<ISystemEvent> _dispatcher;
    private readonly ICardFactory _cardFactory;

    public GameState(
        GameId gameId,
        IEventDispatcher<ISystemEvent> dispatcher,
        ICardFactory cardFactory)
    {
        _gameId = gameId;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;

        ArgumentNullException.ThrowIfNull(cardFactory);
        _cardFactory = cardFactory;

        Register();
    }

    private void Register() => _dispatcher.Register<CardInstanceCreated>(OnCardInstanceCreatedAsync);

    private async Task<IReadOnlyCollection<ISystemEvent>> OnCardInstanceCreatedAsync(
        CardInstanceCreated @event,
        CancellationToken token)
    {
        if (@event.GameId != _gameId)
        {
            return [];
        }

        var card = await _cardFactory.CreateAsync(@event.TemplateId, @event.CardId, token);

        _cards[@event.CardId] = card;

        return [];
    }
}
