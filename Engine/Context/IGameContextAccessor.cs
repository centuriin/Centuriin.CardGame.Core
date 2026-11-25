namespace Centuriin.CardGame.Core.Engine.Context;

/// <summary>
/// Loads the context required for a game operation.
/// </summary>
/// <typeparam name="TContext">
/// The game context type to load.
/// </typeparam>
public interface IGameContextAccessor<TContext>
    where TContext : IGameContext
{
    /// <summary>
    /// Loads the context associated with a game.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The context associated with the requested game.
    /// </returns>
    public Task<TContext> GetContextAsync(GameId gameId, CancellationToken token);
}