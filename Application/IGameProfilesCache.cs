using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Resolves registered profiles by game or game type.
/// </summary>
public interface IGameProfilesCache
{
    /// <summary>
    /// Retrieves the profile associated with a game or game type.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The registered profile for the selected game type.
    /// </returns>
    public ValueTask<IGameProfile> GetProfileAsync(
        GameId gameId,
        CancellationToken token);

    /// <summary>
    /// Retrieves the profile associated with a game or game type.
    /// </summary>
    /// <param name="gameTypeId">
    /// The identifier of the game type.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The registered profile for the selected game type.
    /// </returns>
    public ValueTask<IGameProfile> GetProfileAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}