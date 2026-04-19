using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Repositories.InMemory;
using Centuriin.CardGame.Core.Common.Templates;
using Centuriin.CardGame.Core.Common.World;

using Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddSerilog(x => x.ReadFrom.Configuration(builder.Configuration))
    .AddCore()
    .AddSingleton<IGameSessionsRepository, GameSessionsRepository>()
    .AddSingleton<IZoneDefinitionsRepository, ZoneDefinitionRepo>()
    .AddSingleton<IDecksRepository, DecksRepo>()
    .AddSingleton<ITemplatesRepository<ZoneTemplate>, ZoneTemplatesRepository>()
    .AddSingleton<ITemplatesRepository<CardTemplate>, DefaultCardTemplatesRepository>()
    .AddSingleton<IGameEventsRepository, GameEventRepository>();

var host = builder.Build();

using var scope1 = host.Services.CreateScope();
var st1 = scope1.ServiceProvider.GetRequiredService<IGameStartupService>();

var p1 = new PlayerId(Guid.NewGuid());
var p2 = new PlayerId(Guid.NewGuid());

await st1.StartupGameAsync(new GameSetup(new(1), [p1, p2]), CancellationToken.None);

scope1.Dispose();

using var scope2 = host.Services.CreateScope();
var st2 = scope2.ServiceProvider.GetRequiredService<IGameStartupService>();

await st2.StartupGameAsync(new GameSetup(new(1), [p1, p2]), CancellationToken.None);

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