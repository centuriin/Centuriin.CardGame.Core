using Centuriin.CardGame.Core.Common.Cards;

namespace Centuriin.CardGame.Core.Common.Engine;

public interface ICardPicker
{
    public ICard PickFrom(IReadOnlyList<ICard> source);
}