namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Defines a game's identifier, type, settings, and players.
/// </summary>
public sealed class GameDescription
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
    /// Gets the game-specific settings.
    /// </summary>
    public IGameSettings GameSettings { get; }

    /// <summary>
    /// Gets the player identifiers in the game description.
    /// </summary>
    public IReadOnlyList<PlayerId> PlayerIds { get; }

    /// <summary>
    /// Initializes a new <see cref="GameDescription"/> instance.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="gameTypeId">
    /// The identifier of the game type.
    /// </param>
    /// <param name="gameSettings">
    /// The game-specific settings.
    /// </param>
    /// <param name="playerIds">
    /// The player identifiers in the required order.
    /// </param>
    public GameDescription(
        GameId gameId,
        GameTypeId gameTypeId, 
        IGameSettings gameSettings,
        IReadOnlyList<PlayerId> playerIds)
    {
        GameId = gameId;
        GameTypeId = gameTypeId;

        ArgumentNullException.ThrowIfNull(gameSettings);
        GameSettings = gameSettings;

        ArgumentNullException.ThrowIfNull(playerIds);
        PlayerIds = playerIds;
    }
}