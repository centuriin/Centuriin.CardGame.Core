using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameEventsRepository
{
    public Task AddAsync(
        IReadOnlyCollection<IGameEvent> eventsBatch, 
        CancellationToken token);
}
