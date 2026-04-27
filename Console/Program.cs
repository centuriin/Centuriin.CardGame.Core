using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Observability;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Repositories.InMemory;
using Centuriin.CardGame.Core.Common.Templates;
using Centuriin.CardGame.Core.Common.World;
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
    .AddSingleton<ICommandValidator, EmptyValidator>()
    .AddSingleton<IGameTypeRepository, GameTypeRepo>()
    .AddSingleton<IGameSessionsRepository, GameSessionsRepository>()
    .AddSingleton<IZoneDefinitionsRepository, ZoneDefinitionRepo>()
    .AddSingleton<IDecksRepository, DecksRepo>()
    .AddSingleton<ITemplatesRepository<ZoneTemplate>, ZoneTemplatesRepository>()
    .AddSingleton<ITemplatesRepository<CardTemplate>, DefaultCardTemplatesRepository>()
    .AddSingleton<IGameEventsRepository, GameEventRepository>();

var host = builder.Build();

await host.StartAsync();

var startup = host.Services.GetRequiredService<IGameStartupService>();
var sessionsRepository = host.Services.GetRequiredService<IGameSessionsRepository>();

var p1 = new PlayerId(Guid.NewGuid());
var p2 = new PlayerId(Guid.NewGuid());

var p3 = new PlayerId(Guid.NewGuid());
var p4 = new PlayerId(Guid.NewGuid());

var g1 = await startup.StartupGameAsync(new GameSetup(new(1), [p1, p2]), CancellationToken.None);

await sessionsRepository.RemoveByGameIdAsync(g1.GameId, CancellationToken.None);

var g2 = await startup.StartupGameAsync(new GameSetup(new(1), [p3, p4]), CancellationToken.None);

await sessionsRepository.RemoveByGameIdAsync(g2.GameId, CancellationToken.None);

// todo
public sealed class ZoneDefinitionRepo : IZoneDefinitionsRepository
{
    public async Task<IReadOnlyCollection<ZoneDefinition>> GetZoneDefinitionsAsync(GameTypeId gameTypeId, CancellationToken token) =>
        [
            new ZoneDefinition(new(1), ZoneScope.Singleton),
            new ZoneDefinition(new(2), ZoneScope.PerPlayer)
        ];
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

public sealed class EmptyValidator : ICommandValidator
{
    public IPrimaryEvent? Validate(ICommand command) => null!;
}