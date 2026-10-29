using Centuriin.CardGame.Core.Common.Root;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameSnapshotsRepository
{
    public Task AddOrUpdateSnapshotAsync(IGame game, CancellationToken token);
}