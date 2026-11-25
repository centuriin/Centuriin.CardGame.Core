using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Application.Events;

/// <summary>
/// Reports that a replica validated a command for external execution.
/// </summary>
public sealed class GameCommandPreparedEvent : IGameApplicationEvent
{
    /// <inheritdoc/>
    public GameId GameId { get; }

    /// <inheritdoc/>
    public GameVersion GameVersion { get; }

    /// <summary>
    /// Gets the command associated with the notification.
    /// </summary>
    public ICommand Command { get; }

    /// <summary>
    /// Initializes a new <see cref="GameCommandPreparedEvent"/> instance.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="gameVersion">
    /// The version associated with the game operation.
    /// </param>
    /// <param name="command">
    /// The command to validate or execute.
    /// </param>
    public GameCommandPreparedEvent(
        GameId gameId,
        GameVersion gameVersion,
        ICommand command)
    {
        GameId = gameId;
        GameVersion = gameVersion;

        ArgumentNullException.ThrowIfNull(command);
        Command = command;
    }
}
