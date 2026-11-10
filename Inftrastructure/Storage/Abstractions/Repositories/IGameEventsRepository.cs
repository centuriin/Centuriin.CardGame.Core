using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

public interface IGameEventsRepository
{
    public Task AddAsync(
        IGameEventsBatch eventsBatch,
        CancellationToken token);
}