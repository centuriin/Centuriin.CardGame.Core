using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Logging;

namespace Centuriin.CardGame.Core.Common.Events;

public abstract class SubscriberBase<TEvent> : ISubscriber<TEvent>
    where TEvent : IGameEvent
{
    private static readonly string s_eventType = typeof(TEvent).Name;

    protected IGameEngineLogger Logger { get; }

    protected SubscriberBase(IGameEngineLogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
    }

    public void OnEvent(TEvent @event, IGameState gameState, ChannelWriter<IGameEvent> writer)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(gameState);
        ArgumentNullException.ThrowIfNull(writer);

        Logger.LogDebug("Handling event {EventType}", s_eventType);

        OnEventCore(@event, gameState, writer);
    }

    protected abstract void OnEventCore(
        TEvent @event,
        IGameState gameState,
        ChannelWriter<IGameEvent> writer);
}
