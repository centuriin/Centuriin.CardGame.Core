using Centuriin.CardGame.Core.Application;
using Centuriin.CardGame.Core.Console;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Templates;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.Root.State;
using Centuriin.CardGame.Core.Extensions.DependencyInjection;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories.InMemory;
using Centuriin.CardGame.Core.Infrastructure.Storage.InMemory;

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
    .UseInMemoryStorage()

    .AddSingleton<IGameTypeRepository, GameTypeRepo>()
    .AddSingleton<IDecksRepository, DecksRepo>()
    .AddSingleton<IGameSnapshotsRepository, GameSnapshotRepo>()
    .AddSingleton<IGameDescriptionRepository, GameDescriptionRepo>()
    .AddSingleton<IGameProfilesRepository, GameProfileRepo>()

    ;

var host = builder.Build();

await host.StartAsync();

var descriptionRepo = (GameDescriptionRepo)host.Services.GetRequiredService<IGameDescriptionRepository>();

var startup = host.Services.GetRequiredService<IGameStartupService>();

var g1 = await startup.StartupGameAsync(
    descriptionRepo.FirstGame,
    CancellationToken.None);

await g1.ExecuteAsync(
    new FakeCommand(g1.GameId, descriptionRepo.FirstGame.PlayerIds[1]),
    CancellationToken.None);

g1.Dispose();

var g2 = await startup.StartupGameAsync(
    descriptionRepo.SecondGame,
    CancellationToken.None);

g2.Dispose();

// todo
public sealed record class FakeCommand(GameId GameId, PlayerId Actor) : ICommand
{
    public IGameEvent YieldGameEvent() => new GameStartedEvent(GameId);
}

public sealed class DecksRepo : IDecksRepository
{
    public async Task<IReadOnlyCollection<CardTemplate>> GetDeckTemplatesAsync(
        GameTypeId gameTypeId,
        PlayerId playerId,
        CancellationToken token)
        => [.. DefaultCardTemplatesRepository.Templates36.Values];
}

public sealed class GameTypeRepo : IGameTypeRepository
{
    public Task<GameTypeId> GetGameTypeIdByKey(string key) =>
        Task.FromResult<GameTypeId>(new(1));
}

public sealed class GameDescriptionRepo : IGameDescriptionRepository
{
    public GameDescription FirstGame { get; } =
        new(
            new(Guid.CreateVersion7()),
            new(1),
            [PlayerId.System, new(Guid.NewGuid()), new(Guid.NewGuid())]);

    public GameDescription SecondGame { get; } =
        new(
            new(Guid.CreateVersion7()),
            new(1),
            [PlayerId.System, new(Guid.NewGuid()), new(Guid.NewGuid())]);

    public Task AddAsync(GameDescription gameDescription, CancellationToken token) => throw new NotImplementedException();

    public async Task<GameDescription?> FindAsync(GameId gameId, CancellationToken token)
    {
        if (gameId == FirstGame.GameId)
        {
            return FirstGame;
        }
        else if (gameId == SecondGame.GameId)
        {
            return SecondGame;
        }

        throw new InvalidOperationException();
    }
}

public sealed class GameSnapshotRepo : IGameSnapshotsRepository
{
    private Snapshot? _snapshot = null;

    public Task AddAsync(
        GameId gameId,
        GameTypeId gameTypeId,
        IReadOnlyInitialStateContext stateContext,
        CancellationToken token)
    {
        _snapshot = new Snapshot()
        {
            GameId = gameId,
            TypeId = gameTypeId,
            Entities = stateContext.Entities,
        };

        return Task.CompletedTask;
    }

    public Task AddOrUpdateSnapshotAsync(IGame game, CancellationToken token) => throw new NotImplementedException();

    public Task<IGameSnapshot?> FindAsync(
        GameId gameId,
        CancellationToken token) => Task.FromResult<IGameSnapshot?>(_snapshot);

    private sealed class Snapshot : IGameSnapshot
    {
        public GameId GameId { get; set; }

        public GameTypeId TypeId { get; set; }

        public GameStatus GameStatus => GameStatus.Created;

        public GameVersion Version => GameVersion.Default;

        public required IReadOnlyEntitiesCollection Entities { get; init; }
    }
}

public sealed class GameProfileRepo : IGameProfilesRepository
{
    public async ValueTask<IGameProfile> GetProfileAsync(
        GameId gameId,
        CancellationToken token) => new MyTestProfile();
}