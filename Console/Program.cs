using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Common.ECS.Templates;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Observability;
using Centuriin.CardGame.Core.Common.Storage.Repositories;
using Centuriin.CardGame.Core.Common.Storage.Repositories.InMemory;
using Centuriin.CardGame.Core.Common.World;
using Centuriin.CardGame.Core.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry.Trace;

using Serilog;

var cards = new List<Card>();

var copy = new EntityCopy<Card>(new(new(111)));

cards.Add(copy!);


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
    .AddSingleton<IGameConfigurationRepository, GameConfigRepo>();

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
    new FakeCommand(g1.Id, p1),
    CancellationToken.None);

await sessionsRepository.RemoveByGameIdAsync(g1.Id, CancellationToken.None);

var g2 = await startup.StartupGameAsync(
    new GameSetup(new(1), [PlayerId.System, p3, p4]),
    CancellationToken.None);

await g2.AbortAsync(CancellationToken.None);
await sessionsRepository.RemoveByGameIdAsync(g2.Id, CancellationToken.None);

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

public sealed class GameConfigRepo : IGameConfigurationRepository
{
    public async ValueTask<IGameConfiguration?> FindAsync(
        GameTypeId gameTypeId,
        CancellationToken token) => null;
}