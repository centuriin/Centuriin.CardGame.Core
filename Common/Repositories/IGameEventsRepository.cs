using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IGameEventsRepository
{
    public Task AddAsync(IGameEvent @event, CancellationToken token);
}
