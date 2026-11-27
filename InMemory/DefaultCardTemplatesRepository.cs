using System.Collections.Frozen;

using Centuriin.CardGame.Core.Engine.ECS.Components.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.InMemory;

/// <summary>
/// Provides standard 52-card and 36-card deck templates held in memory.
/// </summary>
public sealed class DefaultCardTemplatesRepository
{
    /// <summary>
    /// Gets the 52 standard card templates indexed by template identifier.
    /// </summary>
    public static FrozenDictionary<TemplateId, CardTemplate> Templates52 { get; } =
        Enumerable.Range(1, 4)
            .Select(suit =>
                Enumerable.Range(2, 13)
                    .Select(rank =>
                        new CardTemplate(
                            new TemplateId(14 * suit + rank),
                            [
                                new SuitComponent((CardSuit)suit),
                                new RankComponent((CardRank)rank),
                            ])))
            .SelectMany(x => x)
            .ToFrozenDictionary(k => k.Id);

    /// <summary>
    /// Gets the 36 standard card templates indexed by template identifier.
    /// </summary>
    public static FrozenDictionary<TemplateId, CardTemplate> Templates36 { get; } =
        Templates52.Values
            .Where(template => template.Components.OfType<RankComponent>().Single().Rank >= CardRank.Six)
            .ToFrozenDictionary(template => template.Id);
}
