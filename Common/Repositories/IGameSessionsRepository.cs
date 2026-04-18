using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IGameSessionsRepository
{
    public ValueTask<IGame> GetGameById(GameId gameId, CancellationToken token);

    public ValueTask AddAsync(IGameSession session, CancellationToken token);

    public ValueTask RemoveByGameIdAsync(GameId gameId, CancellationToken token);
}