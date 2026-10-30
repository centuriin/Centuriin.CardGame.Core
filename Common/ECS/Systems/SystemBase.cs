using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.ECS.Systems;

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

    protected void ValidateAndLog<TEvent>(TEvent @event)
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug("Handling {Event}", typeof(TEvent).Name);
        }
    }
}