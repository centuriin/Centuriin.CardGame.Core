using Centuriin.CardGame.Core.Common.ECS.Templates;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IDecksRepository
{
    public Task<IReadOnlyCollection<TemplateId>> GetDeckTemplateIdsAsync(
        GameTypeId gameTypeId,
        PlayerId playerId,
        CancellationToken token);
}