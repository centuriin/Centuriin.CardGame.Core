namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;

/// <summary>
/// Defines the gameplay purpose of a zone.
/// </summary>
public enum ZoneRole
{
    /// <summary>
    /// No value has been selected.
    /// </summary>
    None,
    /// <summary>
    /// A zone used as a deck.
    /// </summary>
    Deck,
    /// <summary>
    /// A zone holding a player's hand.
    /// </summary>
    Hand,
    /// <summary>
    /// A zone used as a table slot.
    /// </summary>
    Slot
}