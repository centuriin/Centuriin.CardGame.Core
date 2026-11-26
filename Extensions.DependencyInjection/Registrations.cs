using System.Reflection;

using Centuriin.CardGame.Core.Application;
using Centuriin.CardGame.Core.Application.Context;
using Centuriin.CardGame.Core.Application.Events;
using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.ECS.Templates;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Loading;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;
using Centuriin.CardGame.Core.Engine.Observability.Logging;
using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.Root.State;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Registers the engine, application services, and game-scoped factories.
/// </summary>
public static class Registrations
{
    /// <summary>
    /// Registers the common engine services, application services, contexts, and factories.
    /// </summary>
    /// <param name="services">
    /// The service collection to configure.
    /// </param>
    /// <returns>
    /// The service collection for further configuration.
    /// </returns>
    /// <remarks>
    /// The host must also register game profiles, game-specific initializers, the required repositories,
    /// and logging services. A restoration context accessor is required when restoration is used.
    /// </remarks>
    public static IServiceCollection AddCardGameCore(this IServiceCollection services) =>
        services
            .AddSingleton(typeof(ICoreLogger<>), typeof(CoreLoggerAdapter<>))
            .AddApplicationDispatcher<IGameApplicationEvent>(
                AppDomain.CurrentDomain.GetAssemblies())

            .AddSingleton<IGameSessionsCache, InMemoryGameSessionsCache>()
            .AddSingleton<IGameProfilesCache, InMemoryGameProfilesCache>()
            .AddScoped<IGameStartupService, GameStartupService>()

            .AddScoped(typeof(IModulable<>), typeof(ModuleAccessor<>))

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
        services // Todo rework
            .AddScoped<IGameInitializer, GameInitializer>()
            .AddKeyedScoped<IEntityInitializer, PlayersInitializer>("drunkard")
            .AddKeyedScoped<IEntityInitializer, ZonesInitializer>("drunkard")
            .AddKeyedScoped<IEntityInitializer, DecksInitializer>("drunkard");

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

            .AddScoped<GameEventCoordinator>()
            .AddScoped<IGameEventApplier>(sp =>
                sp.GetRequiredService<GameEventCoordinator>())
            .AddScoped<IGameEventProducer>(sp =>
                sp.GetRequiredService<GameEventCoordinator>())

            .AddScoped<GameEventDispatcher>()
            .AddScoped<IConsumingSubscriberDispatcher>(sp =>
                sp.GetRequiredService<GameEventDispatcher>())
            .AddScoped<IProducingSubscriberDispatcher>(sp =>
                sp.GetRequiredService<GameEventDispatcher>())

            .AddScoped<GameState>()
            .AddScoped<IGameState, GameState>(sp =>
                sp.GetRequiredService<GameState>())
            .AddScoped<IReadOnlyGameState>(sp =>
                sp.GetRequiredService<GameState>())

            .AddScoped<ITurnAutomat, TurnAutomat>();

    private static IServiceCollection AddFactories(this IServiceCollection services) =>
        services
            .AddScoped<IEntityIdProvider, EntityIdProvider>()
            .AddScoped<IEntityFactory<Zone, ZoneTemplate>, ZoneFactory>()
            .AddScoped<IEntityFactory<Card, CardTemplate>, CardFactory>()
            .AddScoped<IEntityFactory<Player, PlayerTemplate>, PlayerFactory>()

            .AddSingleton<IGameSessionFactory, GameSessionFactory>()

            .AddScoped<ISystemFactory, SystemFactory>()
            .AddScoped<ICommandRuleFactory, RuleFactory>()
            .AddScoped<IGameFactory, GameFactory>();

    private static IServiceCollection AddApplicationDispatcher<TEvent>(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var modules = assemblies.Distinct().ToArray();

        var contracts = modules
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type =>
                type.IsClass
                && !type.IsAbstract
                && !type.ContainsGenericParameters
                && typeof(TEvent)
                    .IsAssignableFrom(type))
            .Select(type =>
                typeof(IApplicationHandler<>).MakeGenericType(type))
            .ToArray();

        services.Scan(scan => scan
            .FromAssemblies(modules)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IApplicationHandler<>))
                .Where(type => !type.ContainsGenericParameters))
            .As(handlerType => contracts.Where(
                contract => contract.IsAssignableFrom(handlerType)))
            .WithScopedLifetime());

        services.AddSingleton<
            IApplicationDispatcher<TEvent>,
            ApplicationDispatcher<TEvent>>();

        return services;
    }
}
