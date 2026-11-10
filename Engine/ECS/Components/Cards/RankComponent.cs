using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;

namespace Centuriin.CardGame.Core.Engine.ECS.Components.Cards;

/// <summary>
/// Rank component.
/// </summary>
/// <param name="Rank">
/// Card rank.
/// </param>
public sealed record class RankComponent(CardRank Rank) : ComponentBase;