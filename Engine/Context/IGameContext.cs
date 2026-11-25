namespace Centuriin.CardGame.Core.Engine.Context;

/// <summary>
/// Associates loading or initialization data with a game.
/// </summary>
public interface IGameContext
{
    /// <summary>
    /// Gets the identifier of the associated game.
    /// </summary>
    public GameId GameId { get; }
}