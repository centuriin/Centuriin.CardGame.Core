using Centuriin.CardGame.Core.Engine.Root;

namespace Centuriin.CardGame.Core.Engine.Storage.Repositories;

public interface IGameSnapshotsRepository
{
    public Task AddOrUpdateSnapshotAsync(IGame game, CancellationToken token);
}