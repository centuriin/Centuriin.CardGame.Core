using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.Engine;

public interface ICardPicker
{
    public Card PickFrom(IReadOnlyList<Card> source);
}