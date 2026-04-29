using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.GameProfiles;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Repositories.InMemory;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

public static class Registrations
{
    public static IServiceCollection AddCardGameCore(this IServiceCollection services) =>
        services
            .AddSingleton(typeof(ICoreLogger<>), typeof(CoreLoggerAdapter<>))
            .AddSingleton<IGameStartupService, GameStartupService>()

            .AddBootStrappers()

            .AddGameLoop()

            .AddFactories()

            .AddGameProfiles()

            .AddStorage();

    private static IServiceCollection AddBootStrappers(this IServiceCollection services) =>
        services
            .AddScoped<ICommandValidatationConfigurator, CommandValidatationConfigurator>()
            .AddScoped<IGamePipelineConfigurator, GamePipelineConfigurator>()

            .AddSingleton<IGameLoader, ClassicPlayersLoader>()
            .AddSingleton<IGameLoader, ZonesLoader>()
            .AddSingleton<IGameLoader, DecksLoader>();

    private static IServiceCollection AddGameLoop(this IServiceCollection services) =>
        services
            .AddScoped<IGameEventBus, GameEventBus>()
            .AddScoped<IGameEventBusReader>(sp => sp.GetRequiredService<IGameEventBus>())
            .AddScoped<IGameEventBusWriter>(sp => sp.GetRequiredService<IGameEventBus>())

            .AddScoped<CommandValidator>()
            .AddScoped<ICommandValidator>(sp => sp.GetRequiredService<CommandValidator>())
            .AddScoped<IConfigurableCommandValidator>(sp => sp.GetRequiredService<CommandValidator>())

            .AddScoped<IGameEventApplier, GameEventApplier>()
            .AddScoped<IEventDispatcher, EventDispatcher>()
            .AddScoped<ITurnAutomat, TurnAutomat>()
            .AddScoped<IGameState, GameState>();

    private static IServiceCollection AddStorage(this IServiceCollection services) =>
        services
            .AddSingleton<IGameProfilesRepository, GameProfilesRepository>();

    private static IServiceCollection AddFactories(this IServiceCollection services) =>
        services
            .AddSingleton<IZoneFactory, ZoneFactory>()
            .AddSingleton<ICardFactory, CardFactory>()
            .AddSingleton<IGameSessionFactory, GameSessionFactory>()

            .AddScoped<ISystemFactory, SystemFactory>()
            .AddScoped<IRuleFactory, RuleFactory>()
            .AddScoped<IGameFactory, GameFactory>();

    private static IServiceCollection AddGameProfiles(this IServiceCollection services) =>
        services
            .Scan(s => s
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo<IGameProfile>(), publicOnly: true)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
}