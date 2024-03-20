using System.ComponentModel;

using Centuriin.CardGame.Core.Cards;

namespace Centuriin.CardGame.Core.Common.Cards;

/// <summary>
/// Представляет обычную игральную карту.
/// </summary>
public sealed class Card : ICard
{
    /// <summary>
    /// Масть карты.
    /// </summary>
    public CardSuit Suit { get; }

    /// <summary>
    /// Тип карты.
    /// </summary>
    public CardType Type { get; }

    /// <summary>
    /// Создает новый объект типа <see cref="Card"/>.
    /// </summary>
    /// <param name="suit">
    /// Масть карты.
    /// </param>
    /// <param name="type">
    /// Тип карты.
    /// </param>
    /// <exception cref="InvalidEnumArgumentException">
    /// Если перечисления имели неверные значения.
    /// </exception>
    public Card(CardSuit suit, CardType type)
    {
        if (!Enum.IsDefined(suit))
            throw new InvalidEnumArgumentException(nameof(suit), (byte)suit, typeof(CardSuit));
        Suit = suit;

        if (!Enum.IsDefined(type))
            throw new InvalidEnumArgumentException(nameof(type), (byte)type, typeof(CardType));
        Type = type;
    }

    /// <inheritdoc/>
    public bool Equals(ICard? other) =>
        other is Card card
        && Suit == card.Suit
        && Type == card.Type;
}
