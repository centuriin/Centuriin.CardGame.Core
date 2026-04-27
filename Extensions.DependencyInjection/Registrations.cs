using Centuriin.CardGame.Core.Common;
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

            .AddScoped<IGameEventBus, GameEventBus>()
            .AddScoped<IEventDispatcher, EventDispatcher>()
            .AddScoped<ITurnAutomat, TurnAutomat>()
            .AddScoped<IGameState, GameState>()
            .AddScoped<IGamePipelineBuilder, GamePipelineBuilder>()

            .AddStorage()
            .AddLoaders()
            .AddFactories()
            .AddGameProfiles();

    private static IServiceCollection AddStorage(this IServiceCollection services) =>
        services
            .AddSingleton<IGameProfilesRepository, GameProfilesRepository>();

    private static IServiceCollection AddLoaders(this IServiceCollection services) =>
        services
            .AddSingleton<IGameLoader, ClassicPlayersLoader>()
            .AddSingleton<IGameLoader, ZonesLoader>()
            .AddSingleton<IGameLoader, DecksLoader>();

    private static IServiceCollection AddFactories(this IServiceCollection services) =>
        services
            .AddSingleton<IZoneFactory, ZoneFactory>()
            .AddSingleton<ICardFactory, CardFactory>()
            .AddSingleton<IGameSessionFactory, GameSessionFactory>()
            .AddScoped<ISystemFactory, SystemFactory>();

    private static IServiceCollection AddGameProfiles(this IServiceCollection services) =>
        services
            .Scan(s => s
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo<IGameProfile>(), publicOnly: true)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
}