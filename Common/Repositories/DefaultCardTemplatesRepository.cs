using System.Collections.Frozen;

using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.Repositories;

public sealed class DefaultCardTemplatesRepository : ICardTemplatesRepository
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

    public Task<CardTemplate> GetByIdAsync(TemplateId templateId, CancellationToken token) => 
        Task.FromResult(Templates36[templateId]);
    public Task<IReadOnlyCollection<CardTemplate>> GetTemplatesByGameTypeAsync(
        GameTypeId gameTypeId,
        CancellationToken token) => 
        Task.FromResult<IReadOnlyCollection<CardTemplate>>(Templates36.Values);
}
