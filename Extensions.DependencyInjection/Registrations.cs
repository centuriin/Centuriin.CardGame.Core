using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;
using Centuriin.CardGame.Core.Common.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.GameProfiles;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Server;
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
            .AddScoped<IGameEventBusReader>(sp => sp.GetRequiredService<IGameEventBus>())
            .AddScoped<IGameEventBusWriter>(sp => sp.GetRequiredService<IGameEventBus>())

            .AddScoped<ICommandValidator, CommandValidator>()

            .AddScoped<IGameEventApplier, GameEventApplier>()
            .AddScoped(typeof(IDispatcher<>), typeof(Dispatcher<>))
            .AddSingleton(typeof(IAsyncDispatcher<>), typeof(AsyncDispatcher<>))
            .AddScoped<ITurnAutomat, TurnAutomat>()
            .AddScoped<IGameState, GameState>();

    private static IServiceCollection AddStorage(this IServiceCollection services) =>
        services
            .AddSingleton<IGameProfilesRepository, GameProfilesRepository>();

    private static IServiceCollection AddFactories(this IServiceCollection services) =>
        services
            .AddScoped<IZoneFactory, ZoneFactory>()
            .AddScoped<ICardFactory, CardFactory>()

            .AddSingleton<IGameSessionFactory, GameSessionFactory>()
            
            .AddScoped<ICommandHandlerFactory, CommandHandlerFactory>()
            .AddScoped<ISystemFactory, SystemFactory>()
            .AddScoped<ICommandRuleFactory, RuleFactory>()
            .AddScoped<IGameFactory, GameFactory>();

    private static IServiceCollection AddGameProfiles(this IServiceCollection services) =>
        services
            .AddSingleton<IGameConfigurationFactory, GameConfigurationFactory>()
            .AddScoped<IGameConfigurationContext,  GameConfigurationContext>()
            .Scan(s => s
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo<IGameProfile>(), publicOnly: true)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
}
