using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameEventsRepository
{
    public Task AddAsync(
        IGameEventsBatch eventsBatch, 
        CancellationToken token);
}
