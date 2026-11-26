using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Loading;

/// <summary>
/// Loads initial entities into game state and restores the entity identifier sequence.
/// </summary>
public sealed class EntitiesLoader : IGameLoader
{
    private readonly IGameState _gameState;
    private readonly IEntityIdProvider _entityIdProvider;

    private readonly IGameContextAccessor<IReadOnlyInitialStateContext> _gameContextAccessor;

    /// <summary>
    /// Initializes a new <see cref="EntitiesLoader"/> instance.
    /// </summary>
    /// <param name="gameState">
    /// The game state used by the operation.
    /// </param>
    /// <param name="entityIdProvider">
    /// The provider used to allocate entity identifiers.
    /// </param>
    /// <param name="gameContextAccessor">
    /// The accessor used to load initial entity state.
    /// </param>
    public EntitiesLoader(
        IGameState gameState,
        IEntityIdProvider entityIdProvider,
        IGameContextAccessor<IReadOnlyInitialStateContext> gameContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(entityIdProvider);
        _entityIdProvider = entityIdProvider;

        ArgumentNullException.ThrowIfNull(gameContextAccessor);
        _gameContextAccessor = gameContextAccessor;
    }

    /// <inheritdoc/>
    public async Task LoadAsync(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var context = await _gameContextAccessor.GetContextAsync(gameId, token);

        _gameState.Entities.Add(context.Entities);

        _entityIdProvider.SetInitialId(context.LastEntityId);
    }
}
