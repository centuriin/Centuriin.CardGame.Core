using System.Collections.Frozen;

using Centuriin.CardGame.Core.Engine.ECS.Components.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories.InMemory;

public sealed class DefaultCardTemplatesRepository
{
    public static FrozenDictionary<TemplateId, CardTemplate> Templates36 { get; } =
        Enumerable.Range(1, 4)
            .Select(suit =>
                Enumerable.Range(6, 9)
                    .Select(rank =>
                        new CardTemplate(
                            new TemplateId(14 * suit + rank),
                            [
                                new SuitComponent((CardSuit)suit),
                                new RankComponent((CardRank)rank),
                            ])))
            .SelectMany(x => x)
            .ToFrozenDictionary(k => k.Id);
}
