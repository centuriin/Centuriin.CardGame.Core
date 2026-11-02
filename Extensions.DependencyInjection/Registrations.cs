using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Loading.Configuration;
using Centuriin.CardGame.Core.Common.Loading.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Loading.Configuration.Pipeline;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Root.State;
using Centuriin.CardGame.Core.Common.Storage.Repositories;
using Centuriin.CardGame.Core.Common.Storage.Repositories.InMemory;
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

            .AddBootStrappers()

            .AddGameLoop()

            .AddFactories()

            .AddGameProfiles()

            .AddStorage();

    private static IServiceCollection AddBootStrappers(this IServiceCollection services) =>
        services
            .AddScoped<IGameLoader, PlayersLoader>()
            .AddScoped<IGameLoader, ZonesLoader>()
            .AddScoped<IGameLoader, DecksLoader>();

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
            .AddScoped<IGameState>(sp =>
                sp.GetRequiredService<GameState>())
            .AddScoped<IReadOnlyGameState>(sp =>
                sp.GetRequiredService<GameState>())

            .AddScoped<ITurnAutomat, TurnAutomat>();

    private static IServiceCollection AddStorage(this IServiceCollection services) =>
        services
            .AddSingleton<IGameProfilesRepository, GameProfilesRepository>();

    private static IServiceCollection AddFactories(this IServiceCollection services) =>
        services
            .AddScoped<IZoneFactory, ZoneFactory>()
            .AddScoped<ICardFactory, CardFactory>()

            .AddSingleton<IGameSessionFactory, GameSessionFactory>()

            .AddScoped<ISystemFactory, SystemFactory>()
            .AddScoped<ICommandRuleFactory, RuleFactory>()
            .AddScoped<IGameFactory, GameFactory>();

    private static IServiceCollection AddGameProfiles(this IServiceCollection services) =>
        services
            .AddScoped<IGameConfigurationLoader, GamePipelineInitializer>()
            .AddScoped<IGameConfigurationLoader, GameCommandValidatorLoader>()

            .Scan(s => s
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo<IGameProfile>(), publicOnly: true)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
}
