using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

/// <summary>
/// Exposes game state and the operations supported by authoritative games and replicas.
/// </summary>
/// <remarks>
/// Callers must serialize operations on the same game instance.
/// </remarks>
public interface IGame
{
    /// <summary>
    /// Gets the identifier of the game.
    /// </summary>
    public GameId Id { get; }

    /// <summary>
    /// Gets the version of the latest event batch processed by this game.
    /// </summary>
    public GameVersion Version { get; }

    /// <summary>
    /// Gets the current game state without exposing editing operations.
    /// </summary>
    public IReadOnlyGameState State { get; }

    /// <summary>
    /// Applies the next recorded event batch without producing successor events.
    /// </summary>
    /// <param name="batch">
    /// The ordered batch of domain events.
    /// </param>
    /// <remarks>
    /// The game must be running and the batch version must equal the current version plus one.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// The batch is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The game is not running or the batch version is not the next version.
    /// </exception>
    public void Apply(IGameEventsBatch batch);

    /// <summary>
    /// Starts a created game using its authoritative or replica behavior.
    /// </summary>
    /// <returns>
    /// An <see cref="ExecutedResult"/> for an authoritative game, or a <see cref="PreparedResult"/> for a replica.
    /// </returns>
    /// <remarks>
    /// An authoritative game has already applied the events returned in its result.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// The game has already started or is no longer in the created state.
    /// </exception>
    public IEngineResult Start();

    /// <summary>
    /// Validates and processes a command using the game's authoritative or replica behavior.
    /// </summary>
    /// <param name="command">
    /// The command to validate or execute.
    /// </param>
    /// <returns>
    /// The execution, preparation, or rejection result and its game version.
    /// </returns>
    /// <remarks>
    /// An authoritative game produces and applies events before returning <see cref="ExecutedResult"/>.
    /// A replica returns <see cref="PreparedResult"/> without applying the command's event.
    /// Failed validation returns <see cref="RejectedResult"/> without advancing the version.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// The command is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The command targets another game, the game is not running, or command validation is not configured.
    /// </exception>
    public IEngineResult Execute(ICommand command);
}
