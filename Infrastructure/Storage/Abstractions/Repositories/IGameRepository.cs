using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

/// <summary>
/// Updates stored game version metadata.
/// </summary>
public interface IGameRepository
{
    /// <summary>
    /// Updates the stored version of a game.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="newVersion">
    /// The game version to store.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task UpdateVersion(GameId gameId, GameVersion newVersion);
}
