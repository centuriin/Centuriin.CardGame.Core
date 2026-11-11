using Centuriin.CardGame.Core.Application;
using Centuriin.CardGame.Core.Application.Context;
using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Loading;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commads;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;
using Centuriin.CardGame.Core.Engine.Observability.Logging;
using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.Root.State;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

public static class Registrations
{
    public static IServiceCollection AddCardGameCore(this IServiceCollection services) =>
        services
            .AddSingleton(typeof(ICoreLogger<>), typeof(CoreLoggerAdapter<>))
            .AddSingleton(typeof(IAsyncDispatcher<>), typeof(AsyncDispatcher<>))

            .AddScoped<IGameStartupService, GameStartupService>()

            .AddContexts()
            .AddInitialization()
            .AddLoading()

            .AddGameLoop()

            .AddFactories();

    private static IServiceCollection AddContexts(this IServiceCollection services) =>
        services
            .AddSingleton(
                typeof(IGameContextAccessor<>),
                typeof(ScopedGameContextAccessor<>))

            .AddScoped<InitialStateContextAccessor>()
            .AddKeyedScoped<IGameContextAccessor<IInitialStateContext>>(
                "scoped",
                (sp, _) => sp.GetRequiredService<InitialStateContextAccessor>())
            .AddKeyedScoped<IGameContextAccessor<IReadOnlyInitialStateContext>>(
                "scoped",
                (sp, _) => sp.GetRequiredService<InitialStateContextAccessor>())

            .AddKeyedScoped<
                IGameContextAccessor<IGameProfileContext>,
                GameProfileContextAccessor>("scoped");

    private static IServiceCollection AddInitialization(this IServiceCollection services) =>
        services
            .AddScoped<IGameEntityInitializer, PlayersInitializer>()
            .AddScoped<IGameEntityInitializer, ZonesInitializer>()
            .AddScoped<IGameEntityInitializer, DecksInitializer>();

    private static IServiceCollection AddLoading(this IServiceCollection services) =>
        services
            .Scan(s => s
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo<IGameProfile>(), publicOnly: true)
                .AsImplementedInterfaces()
                .WithSingletonLifetime())

            .AddScoped<IGameLoader, GamePipelineLoader>()
            .AddScoped<IGameLoader, GameCommandValidatorLoader>()
            .AddScoped<IGameLoader, EntitiesLoader>()
                .AddScoped<IEntityLoader, EntityLoader<Player>>()
                .AddScoped<IEntityLoader, EntityLoader<Zone>>()
                .AddScoped<IEntityLoader, EntityLoader<Card>>();

    private static IServiceCollection AddGameLoop(this IServiceCollection services) =>
        services
            .AddScoped<IGameEventBus, GameEventBus>()
            .AddScoped<IGameEventBusReader>(sp =>
                sp.GetRequiredService<IGameEventBus>())
            .AddScoped<IGameEventBusWriter>(sp =>
                sp.GetRequiredService<IGameEventBus>())

            .AddScoped<ICommandValidator, CommandValidator>()

            .AddScoped(typeof(GameEventCoordinator))
            .AddScoped<IGameEventApplier>(sp =>
                sp.GetRequiredService<GameEventCoordinator>())
            .AddScoped<IGameEventProducer>(sp =>
                sp.GetRequiredService<GameEventCoordinator>())

            .AddScoped(typeof(GameEventDispatcher))
            .AddScoped<IConsumingSubscriberDispatcher>(sp =>
                sp.GetRequiredService<GameEventDispatcher>())
            .AddScoped<IProducingSubcriberDispatcher>(sp =>
                sp.GetRequiredService<GameEventDispatcher>())

            .AddScoped(typeof(GameState))
            .AddScoped<IGameState, GameState>(sp =>
                sp.GetRequiredService<GameState>())
            .AddScoped<IReadOnlyGameState>(sp =>
                sp.GetRequiredService<GameState>())

            .AddScoped<ITurnAutomat, TurnAutomat>();

    private static IServiceCollection AddFactories(this IServiceCollection services) =>
        services
            .AddScoped<IZoneFactory, ZoneFactory>()
            .AddScoped<ICardFactory, CardFactory>()

            .AddSingleton<IGameSessionFactory, GameSessionFactory>()

            .AddScoped<ISystemFactory, SystemFactory>()
            .AddScoped<ICommandRuleFactory, RuleFactory>()
            .AddScoped<IGameFactory, GameFactory>();
}
