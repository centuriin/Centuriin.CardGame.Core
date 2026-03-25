using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common;

public interface ICardPicker
{
    public Card PickFrom(IReadOnlyList<Card> source);
}