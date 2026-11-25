namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Describes the lifecycle status of a game.
/// </summary>
public enum GameStatus
{
    /// <summary>
    /// No value has been selected.
    /// </summary>
    None,

    /// <summary>
    /// The game has been created but has not started.
    /// </summary>
    Created,

    /// <summary>
    /// The game is in progress.
    /// </summary>
    Running,

    /// <summary>
    /// The game has completed.
    /// </summary>
    Finished,

    /// <summary>
    /// The game has been stopped before completion.
    /// </summary>
    Aborted
}
