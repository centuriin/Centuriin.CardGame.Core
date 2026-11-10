using Centuriin.CardGame.Core.Engine.Storage.Repositories;

namespace Centuriin.CardGame.Core.Engine.Storage;

public interface IGameUnitOfWork
{
    public IGameRepository GameRepository { get; }

    public IGameEventsRepository GameEventsRepository { get; }

    public Task SaveAsync(CancellationToken token);
}
