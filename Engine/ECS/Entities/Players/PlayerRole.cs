namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

/// <summary>
/// Defines the gameplay roles assigned to a player entity.
/// </summary>
[Flags]
public enum PlayerRole
{
    /// <summary>
    /// No value has been selected.
    /// </summary>
    None = 0,
    /// <summary>
    /// The system player that owns the shared deck.
    /// </summary>
    Bank = 1 << 0,
    /// <summary>
    /// A player participating in the game.
    /// </summary>
    Participant = 1 << 1
}