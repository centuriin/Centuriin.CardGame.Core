using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Common.ECS.Templates;

namespace Centuriin.CardGame.Core.Common.ECS.Entities.Factories;

public interface ICardFactory
{
    public Task<IReadOnlyCollection<Card>> CreateAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token);
}
