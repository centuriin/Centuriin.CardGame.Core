using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.ECS.Components.Cards;

/// <summary>
/// Suit component.
/// </summary>
/// <param name="Suit">
/// Card suit.
/// </param>
public sealed record class SuitComponent(CardSuit Suit) : ComponentBase;