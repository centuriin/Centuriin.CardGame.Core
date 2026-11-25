using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Describes a player's request to create a game with particular settings.
/// </summary>
public sealed class GameRequest
{
    /// <summary>
    /// Gets the identifier of the game type.
    /// </summary>
    public GameTypeId GameTypeId { get; }

    /// <summary>
    /// Gets the external player identifier.
    /// </summary>
    public PlayerId PlayerId { get; }

    /// <summary>
    /// Gets the settings requested for the game.
    /// </summary>
    public IGameSettings Settings { get; }

    /// <summary>
    /// Initializes a new <see cref="GameRequest"/> instance.
    /// </summary>
    /// <param name="gameTypeId">
    /// The identifier of the game type.
    /// </param>
    /// <param name="playerId">
    /// The external player identifier.
    /// </param>
    /// <param name="settings">
    /// The game-specific settings.
    /// </param>
    public GameRequest(
        GameTypeId gameTypeId, 
        PlayerId playerId, 
        IGameSettings settings)
    {
        GameTypeId = gameTypeId;
        PlayerId = playerId;

        ArgumentNullException.ThrowIfNull(settings);
        Settings = settings;
    }
}
