using System.Runtime.InteropServices;

using Centuriin.CardGame.Core.Common.Cards;

namespace Centuriin.CardGame.Core.Common.Engine;

public sealed class GameState
{
    private readonly Random _random = Random.Shared;

    private readonly IReadOnlyDictionary<SpaceId, List<Card>> _cardSpaces;
    private readonly ICardPicker _picker;

    public GameState(
        IReadOnlyDictionary<SpaceId, List<Card>> cardSpaces,
        ICardPicker picker)
    {
        ArgumentNullException.ThrowIfNull(cardSpaces);
        _cardSpaces = cardSpaces;

        ArgumentNullException.ThrowIfNull(picker);
        _picker = picker;
    }

    public Card GetCardFromSpace(SpaceId spaceId)
    {
        var card = _picker.PickFrom(_cardSpaces[spaceId]);

        _ = _cardSpaces[spaceId].Remove(card);

        return card;
    }

    public void AddCardToSpace(SpaceId spaceId, Card card, bool isShuffleEnabled = false)
    {
        var space = _cardSpaces[spaceId];

        space.Add(card);

        if (isShuffleEnabled)
            _random.Shuffle(CollectionsMarshal.AsSpan(space));
    }
}
