using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IGameSnapshotsRepository
{
    public Task AddOrUpdateSnapshotAsync(IGame game, CancellationToken token);
}