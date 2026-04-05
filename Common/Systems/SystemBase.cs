using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Logging;

namespace Centuriin.CardGame.Core.Common.Systems;

public abstract class SystemBase
{
    protected IGameEngineLogger Logger { get; }

    protected SystemBase(IGameEngineLogger logger)
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

        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug("Handling {Event}", typeof(TEvent).Name);
        }
    }
}