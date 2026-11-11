using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

public interface IDecksRepository
{
    public Task<IReadOnlyCollection<CardTemplate>> GetDeckTemplatesAsync(
        GameTypeId gameTypeId,
        PlayerId playerId,
        CancellationToken token);
}