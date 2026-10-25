using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameEventsRepository :
    IAsyncSubscriber<NewGameEventsBatchCreatedEvent>
{
    public Task AddAsync(
        IGameEventsBatch eventsBatch, 
        CancellationToken token);
}
