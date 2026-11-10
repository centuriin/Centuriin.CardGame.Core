using Centuriin.CardGame.Core.Engine.Root;

namespace Centuriin.CardGame.Core.Engine.Storage.Repositories;

public interface IGameSessionsRepository : IDisposable
{
    public ValueTask<IGameSession> GetSessionByIdAsync(GameId gameId, CancellationToken token);

    public ValueTask AddAsync(IGameSession session, CancellationToken token);

    public ValueTask RemoveByGameIdAsync(GameId gameId, CancellationToken token);
}
