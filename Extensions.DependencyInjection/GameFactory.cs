using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.Root.State;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Creates game instances through the game-scoped service provider.
/// </summary>
internal sealed class GameFactory : IGameFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new <see cref="GameFactory"/> instance.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider used to create dependencies.
    /// </param>
    public GameFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public IGame CreateMaster(GameId gameId) =>
        ActivatorUtilities.CreateInstance<MasterGame>(
            _serviceProvider,
            gameId);

    /// <inheritdoc/>
    public IGame CreateReplica(IGameSnapshot snapshot) =>
        ActivatorUtilities.CreateInstance<ReplicaGame>(
            _serviceProvider,
            snapshot.GameId,
            snapshot.TypeId,
            snapshot.GameStatus,
            snapshot.Version);
}
