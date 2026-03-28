using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Players;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IDecksRepository
{
    public Task<IReadOnlyCollection<TemplateId>> GetDeckTemplateIdsAsync(
        GameTypeId gameTypeId,
        PlayerId playerId,
        CancellationToken token);
}
