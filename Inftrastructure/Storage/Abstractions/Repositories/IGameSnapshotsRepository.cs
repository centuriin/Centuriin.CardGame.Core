using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

public interface IGameSnapshotsRepository
{
    public Task AddOrUpdateSnapshotAsync(IGame game, CancellationToken token);

    public Task<IGameSnapshot?> FindAsync(GameId gameId, CancellationToken token);

    public Task AddAsync(
        GameId gameId,
        GameTypeId gameTypeId,
        IReadOnlyInitialStateContext stateContext,
        CancellationToken token);
}