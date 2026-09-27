using Centuriin.CardGame.Core.Common.ECS.Components;

namespace Centuriin.CardGame.Core.Common.ECS.Components.Players;

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