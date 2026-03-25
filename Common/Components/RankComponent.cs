using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Rank component.
/// </summary>
/// <param name="Rank">
/// Card rank.
/// </param>
public sealed record class RankComponent(CardRank Rank) : ComponentBase;