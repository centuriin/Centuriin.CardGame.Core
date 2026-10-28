using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Root;

public abstract class GameEventApplierBase : IGameEventApplier
{
    protected IDispatcher<IGameEvent> Dispatcher { get; }

    protected GameEventApplierBase(IDispatcher<IGameEvent> dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        Dispatcher = dispatcher;
    }

    public virtual void SilentApply(IGameEventsBatch batch)
    {
        ArgumentNullException.ThrowIfNull(batch);

        foreach (var @event in batch)
        {
            Dispatcher.Publish(@event);
        }
    }

    public abstract IGameEventsBatch Apply(IGameEvent @event);
}