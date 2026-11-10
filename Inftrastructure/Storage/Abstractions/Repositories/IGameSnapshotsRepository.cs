using Centuriin.CardGame.Core.Engine.Root;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

public interface IGameSnapshotsRepository
{
    public Task AddOrUpdateSnapshotAsync(IGame game, CancellationToken token);
}