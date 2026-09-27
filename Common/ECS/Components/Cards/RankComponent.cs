using Centuriin.CardGame.Core.Common.ECS.Components;
using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.ECS.Components.Cards;

/// <summary>
/// Rank component.
/// </summary>
/// <param name="Rank">
/// Card rank.
/// </param>
public sealed record class RankComponent(CardRank Rank) : ComponentBase;