using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

public interface ITemplatesRepository<TTemplate>
    where TTemplate : TemplateBase
{
    public Task<IReadOnlyCollection<TTemplate>> GetTemplatesAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}