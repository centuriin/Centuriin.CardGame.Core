namespace Centuriin.CardGame.Core.Engine.Events.Domain;

/// <summary>
/// Identifies the game associated with a domain event.
/// </summary>
public interface IGameEvent
{
    /// <summary>
    /// Gets the identifier of the associated game.
    /// </summary>
    public GameId GameId { get; }
}
