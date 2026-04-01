using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Repositories.InMemory;

public sealed class GameEventRepository : IGameEventsRepository
{
    public readonly List<IGameEvent> Events = [];

    public Task AddAsync(IGameEvent @event, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        Events.Add(@event);

        return Task.CompletedTask;
    }
}
