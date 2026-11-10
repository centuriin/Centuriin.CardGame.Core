using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;

namespace Centuriin.CardGame.Core.Engine.ECS.Components.Cards;

/// <summary>
/// Suit component.
/// </summary>
/// <param name="Suit">
/// Card suit.
/// </param>
public sealed record class SuitComponent(CardSuit Suit) : ComponentBase;