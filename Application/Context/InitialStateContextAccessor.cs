using Application.Initialization;

using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Observability.Logging;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Application.Context;

public sealed class InitialStateContextAccessor :
    IGameContextAccessor<IInitialStateContext>,
    IGameContextAccessor<IReadOnlyInitialStateContext>
{
    private readonly IGameDescriptionRepository _descriptionRepository;
    private readonly IGameSnapshotsRepository _snapshotsRepository;
    private readonly IEnumerable<IGameEntityInitializer> _entityInitializers;
    private readonly ICoreLogger<InitialStateContextAccessor> _logger;

    public InitialStateContextAccessor(
        IGameDescriptionRepository descriptionRepository,
        IGameSnapshotsRepository snapshotsRepository,
        IEnumerable<IGameEntityInitializer> entityInitializers,
        ICoreLogger<InitialStateContextAccessor> logger)
    {
        ArgumentNullException.ThrowIfNull(descriptionRepository);
        _descriptionRepository = descriptionRepository;

        ArgumentNullException.ThrowIfNull(snapshotsRepository);
        _snapshotsRepository = snapshotsRepository;

        ArgumentNullException.ThrowIfNull(entityInitializers);
        _entityInitializers = entityInitializers;

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public async Task<IInitialStateContext> GetContextAsync(
        GameId gameId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var snapshot = await _snapshotsRepository.FindAsync(gameId, token);

        if (snapshot is not null)
        {
            _logger.LogDebug("State context loaded from game snapshot");

            return new InitialStateContext
            {
                GameId = gameId,
                Entities = snapshot.Entities.ToCollection(),
            };
        }

        var gameDescription = await _descriptionRepository.FindAsync(gameId, token)
            ?? throw new InvalidOperationException(
                $"Can't find game description for game: {gameId.Value}");

        var context = new InitialStateContext { GameId = gameId };

        foreach (var initializer in _entityInitializers)
        {
            await initializer.InitializeAsync(
                gameDescription,
                context,
                token);
        }

        await _snapshotsRepository.AddAsync(
            gameDescription.GameId,
            gameDescription.GameTypeId,
            context,
            token);

        _logger.LogDebug("State context initialized and stored");

        return context;
    }

    async Task<IReadOnlyInitialStateContext> IGameContextAccessor<IReadOnlyInitialStateContext>.GetContextAsync(
        GameId gameId,
        CancellationToken token)
        => await GetContextAsync(gameId, token);

    private sealed class InitialStateContext : IInitialStateContext
    {
        public required GameId GameId { get; set; }

        public IEntitiesCollection Entities { get; set; } = new EntitiesCollection();
    }
}
