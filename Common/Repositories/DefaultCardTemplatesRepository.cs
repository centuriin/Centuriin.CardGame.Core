using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Text;

using Centuriin.CardGame.Core.Common.Cards;
using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Repositories;

public sealed class DefaultCardTemplatesRepository
{
    public static FrozenDictionary<TemplateId, CardTemplate> Templates36 { get; } =
        Enumerable.Range(1, 4)
            .Select(suit =>
                Enumerable.Range(6, 9)
                    .Select(rank =>
                        new CardTemplate(
                            new TemplateId(14*suit + rank),
                            [
                                new TemplateComponent(new TemplateId(14*suit + rank)),
                                new SuitComponent((CardSuit)suit),
                                new RankComponent((CardRank)rank),
                            ])))
            .SelectMany(x => x)
            .ToFrozenDictionary(k => k.Id);
}
