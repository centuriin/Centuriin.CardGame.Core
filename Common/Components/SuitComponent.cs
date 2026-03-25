using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Suit component.
/// </summary>
/// <param name="Suit">
/// Card suit.
/// </param>
public sealed record class SuitComponent(CardSuit Suit) : ComponentBase;