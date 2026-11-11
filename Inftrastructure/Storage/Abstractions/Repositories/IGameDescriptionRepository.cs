using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

public interface IGameDescriptionRepository
{
    public Task<GameDescription?> FindAsync(GameId gameId, CancellationToken token);

    public Task AddAsync(GameDescription gameDescription, CancellationToken token);
}
