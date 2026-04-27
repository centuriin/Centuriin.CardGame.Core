namespace Centuriin.CardGame.Core.Common.Components.Players;

/// <summary>
/// Describes link on external (global) player.
/// </summary>
public sealed record class PlayerIdentifierComponent : ComponentBase
{
    public static PlayerIdentifierComponent System { get; } = new(PlayerId.System);

    public PlayerId PlayerId { get; }

    public PlayerIdentifierComponent(PlayerId playerId)
    {
        PlayerId = playerId;
    }
}