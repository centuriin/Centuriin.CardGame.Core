using Centuriin.CardGame.Core.Common.Cards;

namespace Centuriin.CardGame.Core.Common.Engine;

public sealed class RandomCardPicker : ICardPicker
{
    private readonly Random _random = Random.Shared;

    public ICard PickFrom(IReadOnlyList<ICard> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source[_random.Next(source.Count)];
    }
}
