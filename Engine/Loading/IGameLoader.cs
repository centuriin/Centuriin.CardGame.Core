namespace Centuriin.CardGame.Core.Engine.Loading;

/// <summary>
/// Loads one part of a game's configuration or initial state.
/// </summary>
public interface IGameLoader
{
    /// <summary>
    /// Loads configuration or initial state for a game.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task LoadAsync(GameId gameId, CancellationToken token);
}