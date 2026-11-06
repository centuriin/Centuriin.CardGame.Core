using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Common.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Initialization;
using Centuriin.CardGame.Core.Common.Loading;
using Centuriin.CardGame.Core.Common.Loading.Configuration;
using Centuriin.CardGame.Core.Common.Loading.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Loading.Configuration.Pipeline;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Root.State;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

public static class Registrations
{
    public static IServiceCollection AddCardGameCore(this IServiceCollection services) =>
        services
            .AddSingleton(typeof(ICoreLogger<>), typeof(CoreLoggerAdapter<>))
            .AddSingleton(typeof(IAsyncDispatcher<>), typeof(AsyncDispatcher<>))

            .AddScoped<IGameStartupService, GameStartupService>()

            .AddInitialization()
            .AddLoading()

            .AddGameLoop()

            .AddFactories();

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
