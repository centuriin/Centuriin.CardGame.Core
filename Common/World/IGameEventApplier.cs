using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGameEventApplier
{
    public Task<IGameEventUnit> ApplyAsync(IPrimaryEvent @event, CancellationToken token);
}