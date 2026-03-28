using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Cards;

namespace Centuriin.CardGame.Core.Common.Factories;

public interface ICardsFactory
{
    public Task<IReadOnlyCollection<Card>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token);
}
