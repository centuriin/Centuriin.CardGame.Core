using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common;

public sealed class RandomCardPicker : ICardPicker
{
    private readonly Random _random = Random.Shared;

    public Card PickFrom(IReadOnlyList<Card> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source[_random.Next(source.Count)];
    }
}
