using Centuriin.CardGame.Core.Engine.Events.Application;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Storage.Repositories;

public interface IGameEventsRepository :
    IAsyncSubscriber<NewGameEventsBatchCreatedEvent>
{
    public Task AddAsync(
        IGameEventsBatch eventsBatch,
        CancellationToken token);
}
