using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

/// <summary>
/// Creates authoritative games and replicas from game-scoped dependencies.
/// </summary>
public interface IGameFactory
{
    /// <summary>
    /// Creates an authoritative game for the supplied identifier.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <returns>
    /// The created game instance.
    /// </returns>
    public IGame CreateMaster(GameId gameId);

    /// <summary>
    /// Creates a replica for a stored game snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// The stored game snapshot.
    /// </param>
    /// <returns>
    /// The created game instance.
    /// </returns>
    public IGame CreateReplica(IGameSnapshot snapshot);
}