using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Templates;

namespace Centuriin.CardGame.Core.Common.Entities.Factories;

public interface ICardFactory
{
    public Task<IReadOnlyCollection<Card>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token);
}
