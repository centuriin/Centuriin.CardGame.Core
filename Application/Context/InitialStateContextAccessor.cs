using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Observability.Logging;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application.Context;

/// <summary>
/// Loads entity state from a snapshot or initializes and stores the initial entities.
/// </summary>
public sealed class InitialStateContextAccessor :
    IGameContextAccessor<IInitialStateContext>,
    IGameContextAccessor<IReadOnlyInitialStateContext>
{
    private readonly IGameDescriptionRepository _descriptionRepository;
    private readonly IGameSnapshotsRepository _snapshotsRepository;
    private readonly IGameInitializer _gameInitializer;
    private readonly ICoreLogger<InitialStateContextAccessor> _logger;

    /// <summary>
    /// Initializes a new <see cref="InitialStateContextAccessor"/> instance.
    /// </summary>
    /// <param name="descriptionRepository">
    /// The repository used to retrieve game descriptions.
    /// </param>
    /// <param name="snapshotsRepository">
    /// The repository used to load and store snapshots.
    /// </param>
    /// <param name="gameInitializer">
    /// The module adapter used to resolve the game initializer.
    /// </param>
    /// <param name="logger">
    /// The logger used for diagnostics.
    /// </param>
    public InitialStateContextAccessor(
        IGameDescriptionRepository descriptionRepository,
        IGameSnapshotsRepository snapshotsRepository,
        IGameInitializer gameInitializer,
        ICoreLogger<InitialStateContextAccessor> logger)
    {
        ArgumentNullException.ThrowIfNull(descriptionRepository);
        _descriptionRepository = descriptionRepository;

        ArgumentNullException.ThrowIfNull(snapshotsRepository);
        _snapshotsRepository = snapshotsRepository;

        ArgumentNullException.ThrowIfNull(gameInitializer);
        _gameInitializer = gameInitializer;

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IInitialStateContext> GetContextAsync(
        GameId gameId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var snapshot = await _snapshotsRepository.FindAsync(gameId, token);

        if (snapshot is not null)
        {
            _logger.LogDebug("State context loaded from game snapshot");

            return await _gameInitializer.InitializeAsync(snapshot, token);
        }

        var description = await _descriptionRepository.FindAsync(gameId, token)
            ?? throw new InvalidOperationException(
                $"Can't find game description for game: {gameId.Value}");

        var context = await _gameInitializer.InitializeAsync(description, token);

        await _snapshotsRepository.AddAsync(
            description.GameId,
            description.GameTypeId,
            context,
            token);

        _logger.LogDebug("State context initialized and stored");

        return context;
    }

    /// <inheritdoc/>
    async Task<IReadOnlyInitialStateContext> IGameContextAccessor<IReadOnlyInitialStateContext>.GetContextAsync(
        GameId gameId,
        CancellationToken token)
        => await GetContextAsync(gameId, token);
}