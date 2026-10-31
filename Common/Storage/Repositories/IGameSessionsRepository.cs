using Centuriin.CardGame.Core.Common.Root;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameSessionsRepository : IDisposable
{
    public ValueTask<IGameSession> GetSessionByIdAsync(GameId gameId, CancellationToken token);

    public ValueTask AddAsync(IGameSession session, CancellationToken token);

    public ValueTask RemoveByGameIdAsync(GameId gameId, CancellationToken token);
}
