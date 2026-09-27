using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Storage.Repositories;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories.InMemory;

public sealed class GameEventRepository : IGameEventsRepository
{
    public readonly List<IGameEventsBatch> Events = [];

    public Task AddAsync(IGameEventsBatch @event, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        Events.Add(@event);

        return Task.CompletedTask;
    }
}
