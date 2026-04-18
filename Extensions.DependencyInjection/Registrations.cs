using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Extensions.DependencyInjection;

public static class Registrations
{
    public static IServiceCollection AddCore(this IServiceCollection services) =>
        services
            .AddSingleton(typeof(ICoreLogger<>), typeof(CoreLoggerAdapter<>))
            .AddSingleton<IGameSessionFactory, GameFactory>()
            .AddSingleton<IGameStartupService, GameStartupService>()
            .AddScoped<IEventDispatcher, EventDispatcher>()
            .AddScoped<ITurnAutomat, TurnAutomat>()
            .AddScoped<IGameState, GameState>()
            .AddLoaders()
            .AddFactories();

    private static IServiceCollection AddLoaders(this IServiceCollection services) =>
        services
            .AddSingleton<IGameLoader, ClassicPlayersLoader>()
            .AddSingleton<IGameLoader, ZonesLoader>()
            .AddSingleton<IGameLoader, DecksLoader>();

    private static IServiceCollection AddFactories(this IServiceCollection services) =>
        services
            .AddSingleton<IZoneFactory, ZoneFactory>()
            .AddSingleton<ICardFactory, CardFactory>();
}
