using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Coordinates the lifecycle and command execution of a game session.
/// </summary>
/// <remarks>
/// Callers must serialize operations on a session. Dispose the session when it is no longer needed.
/// Removing a session through <see cref="IGameSessionsCache.RemoveByGameIdAsync"/> also disposes it.
/// </remarks>
public interface IGameSession : IDisposable
{
    /// <summary>
    /// Gets the identifier of the associated game.
    /// </summary>
    public GameId GameId { get; }

    /// <summary>
    /// Gets the identifier of the game type.
    /// </summary>
    public GameTypeId GameTypeId { get; }

    /// <summary>
    /// Starts the game and publishes the corresponding application notification.
    /// </summary>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task StartAsync(CancellationToken token);

    /// <summary>
    /// Validates a command, executes it when authoritative, and publishes the resulting notification.
    /// </summary>
    /// <param name="command">
    /// The command to validate or execute.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task ExecuteAsync(ICommand command, CancellationToken token);

    /// <summary>
    /// Loads and applies a restoration batch and publishes the restored game version.
    /// </summary>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    /// <remarks>
    /// The host must register a restoration context accessor and supply the next event batch.
    /// </remarks>
    public Task RestoreAsync(CancellationToken token);
}
