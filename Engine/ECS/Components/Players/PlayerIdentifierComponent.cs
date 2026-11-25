namespace Centuriin.CardGame.Core.Engine.ECS.Components.Players;

/// <summary>
/// Describes link on external (global) player.
/// </summary>
public sealed record class PlayerIdentifierComponent : ComponentBase
{
    /// <summary>
    /// Gets the built-in system player value.
    /// </summary>
    public static PlayerIdentifierComponent System { get; } = new(PlayerId.System);

    /// <summary>
    /// Gets the external player identifier.
    /// </summary>
    public PlayerId PlayerId { get; }

    /// <summary>
    /// Initializes a new <see cref="PlayerIdentifierComponent"/> instance.
    /// </summary>
    /// <param name="playerId">
    /// The external player identifier.
    /// </param>
    public PlayerIdentifierComponent(PlayerId playerId)
    {
        PlayerId = playerId;
    }
}