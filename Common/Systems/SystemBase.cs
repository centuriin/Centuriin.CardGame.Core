using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Systems;

public abstract class SystemBase
{
    protected IGameState GameState { get; }

    protected IGameEventBusWriter EventBusWriter { get; }

    protected ICoreLogger Logger { get; }

    protected SystemBase(
        IGameState gameState,
        IGameEventBusWriter eventBusWriter,
        ICoreLogger logger)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        GameState = gameState;

        ArgumentNullException.ThrowIfNull(eventBusWriter);
        EventBusWriter = eventBusWriter;

        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
    }

    protected void ValidateAndLog<TEvent>(TEvent @event, IGameState gameState, IGameEventBus writer)
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