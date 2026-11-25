using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Commands;

/// <summary>
/// Describes a player's intended action and its initial game event.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Gets the identifier of the associated game.
    /// </summary>
    public GameId GameId { get; }

    /// <summary>
    /// Gets the player who issued the command.
    /// </summary>
    public PlayerId Actor { get; }

    /// <summary>
    /// Creates the initial domain event used to process this command.
    /// </summary>
    /// <returns>
    /// The initial domain event for this command.
    /// </returns>
    public IGameEvent YieldGameEvent();
}
