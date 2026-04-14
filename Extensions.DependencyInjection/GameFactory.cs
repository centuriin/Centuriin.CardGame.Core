using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Systems;

using Microsoft.Extensions.DependencyInjection;

namespace Extensions.DependencyInjection;

public sealed class GameFactory : IGameFactory
{
    private readonly IServiceProvider _serviceProvider;

    public GameFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    public IGame Create(GameSetup setup)
    {
        ArgumentNullException.ThrowIfNull(setup);

        var dispatcher = _serviceProvider.GetRequiredService<IEventDispatcher>();

        //todo dispatcher linked list settings by builder
        dispatcher.Register<GameStartedEvent>(
            new SetupTurnFlowSystem(
                _serviceProvider.GetRequiredService<ICoreLogger<SetupTurnFlowSystem>>()));
        dispatcher.Register<GameStartedEvent>(
            new DealerSystem(
                _serviceProvider.GetRequiredService<ICoreLogger<DealerSystem>>()));
        dispatcher.Register<TurnFlowDefinedEvent>(
            new TurnFlowSystem(
                _serviceProvider.GetRequiredService<ICoreLogger<TurnFlowSystem>>()));
        dispatcher.Register<CardDealtEvent>(
            new CardMovementSystem(
                _serviceProvider.GetRequiredService<ICoreLogger<CardMovementSystem>>()));

        var gameState = new GameState(
            new(Guid.NewGuid()),
            _serviceProvider.GetRequiredService<ITurnAutomat>());

        return ActivatorUtilities.CreateInstance<Game>(_serviceProvider, gameState, dispatcher);
    }
}
