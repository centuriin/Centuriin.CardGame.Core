using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Creates and starts a session for a game description.
/// </summary>
public interface IGameStartupService
{
    /// <summary>
    /// Creates, caches, and starts a session for the supplied game description.
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
    public Task<IGameSession> StartupGameAsync(
        GameDescription description,
        CancellationToken token);
}