using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Context;
using Centuriin.CardGame.Core.Common.ECS.Templates;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Loading.Configuration;
using Centuriin.CardGame.Core.Common.Observability;
using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Storage.Repositories;
using Centuriin.CardGame.Core.Common.Storage.Repositories.InMemory;
using Centuriin.CardGame.Core.Common.World;
using Centuriin.CardGame.Core.Console;
using Centuriin.CardGame.Core.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry.Trace;

using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddOpenTelemetry()
        .WithTracing(x =>
            x.AddSource(Telemetry.ACTIVITY_SOURCE_NAME)
             .AddConsoleExporter())

    .Services
    .AddSerilog(x => x.ReadFrom.Configuration(builder.Configuration))

    .AddCardGameCore()
    .AddSingleton<IGameTypeRepository, GameTypeRepo>()
    .AddSingleton<IGameSessionsRepository, GameSessionsRepository>()
    .AddSingleton<IZonesRepository, ZoneDefinitionRepo>()
    .AddSingleton<IDecksRepository, DecksRepo>()
    .AddSingleton<ITemplatesRepository<ZoneTemplate>, ZoneTemplatesRepository>()
    .AddSingleton<ITemplatesRepository<CardTemplate>, DefaultCardTemplatesRepository>()
    .AddSingleton<IGameEventsRepository, GameEventRepository>()

    .AddSingleton(typeof(ContextAccessor))
    .AddSingleton<IGameContextAccessor<IRestorationContext>>(sp =>
        sp.GetRequiredService<ContextAccessor>())
    .AddSingleton<IGameContextAccessor<IInitialStateContext>>(sp =>
        sp.GetRequiredService<ContextAccessor>())
    .AddSingleton<IGameContextAccessor<IReadOnlyInitialStateContext>>(sp =>
        sp.GetRequiredService<ContextAccessor>())
    .AddSingleton<IGameContextAccessor<IGameProfileContext>>(sp =>
        sp.GetRequiredService<ContextAccessor>());

var host = builder.Build();

await host.StartAsync();

var startup = host.Services.GetRequiredService<IGameStartupService>();
var sessionsRepository = host.Services.GetRequiredService<IGameSessionsRepository>();

var p1 = new PlayerId(Guid.NewGuid());
var p2 = new PlayerId(Guid.NewGuid());

var p3 = new PlayerId(Guid.NewGuid());
var p4 = new PlayerId(Guid.NewGuid());

var g1 = await startup.StartupGameAsync(
    new GameSetup(new(1), [PlayerId.System, p1, p2]),
    CancellationToken.None);

await g1.ExecuteAsync(
    new FakeCommand(g1.GameId, p1),
    CancellationToken.None);

await sessionsRepository.RemoveByGameIdAsync(g1.GameId, CancellationToken.None);

var g2 = await startup.StartupGameAsync(
    new GameSetup(new(1), [PlayerId.System, p3, p4]),
    CancellationToken.None);

await sessionsRepository.RemoveByGameIdAsync(g2.GameId, CancellationToken.None);

// todo
public sealed record class FakeCommand(GameId GameId, PlayerId Actor) : ICommand
{
    public IGameEvent YieldGameEvent() => new GameStartedEvent(GameId);
}

public sealed class ZoneDefinitionRepo : IZonesRepository
{
    public Task<IReadOnlyCollection<TemplateId>> GetZoneTemplateIdsAsync(GameTypeId gameTypeId, CancellationToken token) =>
        Task.FromResult<IReadOnlyCollection<TemplateId>>([new(1), new(2)]);
}

public sealed class DecksRepo : IDecksRepository
{
    public async Task<IReadOnlyCollection<TemplateId>> GetDeckTemplateIdsAsync(GameTypeId gameTypeId, PlayerId playerId, CancellationToken token) =>
        [.. DefaultCardTemplatesRepository.Templates36.Keys];
}

public sealed class GameTypeRepo : IGameTypeRepository
{
    public Task<GameTypeId> GetGameTypeIdByKey(string key) =>
        Task.FromResult<GameTypeId>(new(1));
}

public sealed class ContextAccessor :
    IGameContextAccessor<IRestorationContext>,
    IGameContextAccessor<IInitialStateContext>,
    IGameContextAccessor<IReadOnlyInitialStateContext>,
    IGameContextAccessor<IGameProfileContext>
{
    private readonly Dictionary<GameId, Context> _contexts = new();

    Task<IGameProfileContext> IGameContextAccessor<IGameProfileContext>.GetContextAsync(
        GameId gameId,
        CancellationToken token)
    {
        if (!_contexts.TryGetValue(gameId, out var context))
        {
            context = new Context() { GameId = gameId };
            _contexts[gameId] = context;
        }

        return Task.FromResult<IGameProfileContext>(context);
    }

    Task<IRestorationContext> IGameContextAccessor<IRestorationContext>.GetContextAsync(
        GameId gameId,
        CancellationToken token)
        => throw new NotImplementedException();

    Task<IInitialStateContext> IGameContextAccessor<IInitialStateContext>.GetContextAsync(
        GameId gameId,
        CancellationToken token)
    {
        if (!_contexts.TryGetValue(gameId, out var context))
        {
            context = new Context() { GameId = gameId };
            _contexts[gameId] = context;
        }

        return Task.FromResult<IInitialStateContext>(context);
    }

    Task<IReadOnlyInitialStateContext> IGameContextAccessor<IReadOnlyInitialStateContext>.GetContextAsync(
        GameId gameId,
        CancellationToken token)
    {
        if (!_contexts.TryGetValue(gameId, out var context))
        {
            context = new Context() { GameId = gameId };
            _contexts[gameId] = context;
        }

        return Task.FromResult<IReadOnlyInitialStateContext>(context);
    }

    private sealed class Context :
        IInitialStateContext,
        IGameProfileContext
    {
        public required GameId GameId { get; init; }

        public IEntitiesCollection Entities { get; } = new EntitiesCollection();

        public IGameProfile Profile { get; } = new MyTestProfile();
    }
}