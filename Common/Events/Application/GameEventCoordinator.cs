using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class GameEventCoordinator : IGameEventProducer, IGameEventApplier
{
    private readonly IGameState _gameState;
    private readonly IGameEventBus _eventBus;
    private readonly IDispatcher<IApplicationEvent> _dispatcher;

    public GameEventCoordinator(
        IGameState gameState,
        IGameEventBus eventBus,
        IDispatcher<IApplicationEvent> dispatcher)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(eventBus);
        _eventBus = eventBus;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;
    }

    public void Apply<TEvent>(TEvent @event) 
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        var applyEvent = new ApplyGameEvent<TEvent>(@event, _gameState);

        _dispatcher.Publish(applyEvent);
    }
    
    public IReadOnlyList<IGameEvent> Produce<TEvent>(TEvent @event) 
        where TEvent : IGameEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        var applied = new List<IGameEvent>()
        {
            @event
        };

        Prod produceEvent = new ProduceGameEvent<TEvent>(@event, _gameState, _eventBus);

        _dispatcher.Publish(produceEvent);



        return applied;
    }
}
