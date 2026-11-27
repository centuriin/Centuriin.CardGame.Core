using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Resolves registered profiles by game or game type.
/// </summary>
public interface IGameModulesCache
{
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
    public ValueTask<IGameModule> GetModuleAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}