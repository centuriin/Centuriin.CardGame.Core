using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Extensions.DependencyInjection;

public sealed class GameFactory : IGameSessionFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GameFactory(IServiceScopeFactory serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceScopeFactory = serviceProvider;
    }

    public IGameSession Create(GameSetup setup)
    {
        ArgumentNullException.ThrowIfNull(setup);

        var scope = _serviceScopeFactory.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        var dispatcher = serviceProvider.GetRequiredService<IEventDispatcher>();

        //todo dispatcher linked list settings by builder
        dispatcher.Register<GameStartedEvent>(
            new SetupTurnFlowSystem(
                serviceProvider.GetRequiredService<ICoreLogger<SetupTurnFlowSystem>>()));
        dispatcher.Register<GameStartedEvent>(
            new DealerSystem(
                serviceProvider.GetRequiredService<ICoreLogger<DealerSystem>>()));
        dispatcher.Register<TurnFlowDefinedEvent>(
            new TurnFlowSystem(
                serviceProvider.GetRequiredService<ICoreLogger<TurnFlowSystem>>()));
        dispatcher.Register<CardDealtEvent>(
            new CardMovementSystem(
                serviceProvider.GetRequiredService<ICoreLogger<CardMovementSystem>>()));

        var game = ActivatorUtilities.CreateInstance<Game>(serviceProvider, new GameId(Guid.NewGuid()), dispatcher);

        return new GameSession(game, scope);
    }
}
