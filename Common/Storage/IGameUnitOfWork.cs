using Centuriin.CardGame.Core.Common.Storage.Repositories;

namespace Centuriin.CardGame.Core.Common.Storage;

public interface IGameUnitOfWork
{
    public IGameRepository GameRepository { get; }

    public IGameEventsRepository GameEventsRepository { get; }

    public Task SaveAsync(CancellationToken token);
}
