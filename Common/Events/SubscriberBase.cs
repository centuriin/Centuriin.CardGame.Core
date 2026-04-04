using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Logging;

namespace Centuriin.CardGame.Core.Common.Events;

public abstract class SubscriberBase
{
    protected IGameEngineLogger Logger { get; }

    protected SubscriberBase(IGameEngineLogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
    }

    protected void ValidateAndLog<TEvent>(TEvent @event, IGameState gameState, ChannelWriter<IGameEvent> writer)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(gameState);
        ArgumentNullException.ThrowIfNull(writer);

        Logger.LogDebug("Handling {Event}", typeof(TEvent).Name);
    }
}