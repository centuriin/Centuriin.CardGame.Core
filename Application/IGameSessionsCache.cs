using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

public interface IGameSessionsCache
{
    public ValueTask<IGameSession> GetSessionByIdAsync(GameId gameId, CancellationToken token);

    public ValueTask AddAsync(IGameSession session, CancellationToken token);

    public ValueTask RemoveByGameIdAsync(GameId gameId, CancellationToken token);
}
