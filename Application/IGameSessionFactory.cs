using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Creates game sessions and their game-scoped dependencies.
/// </summary>
public interface IGameSessionFactory
{
    /// <summary>
    /// Creates and loads a game session without starting it.
    /// </summary>
    /// <param name="description">
    /// The game description used to create the session.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The created game session.
    /// </returns>
    public ValueTask<IGameSession> CreateAsync(
        GameDescription description,
        CancellationToken token);
}