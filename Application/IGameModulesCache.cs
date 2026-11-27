using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Resolves modules associated with the host's game types.
/// </summary>
public interface IGameModulesCache
{
    /// <summary>
    /// Retrieves the module assigned to a game type by the host.
    /// </summary>
    /// <param name="gameTypeId">
    /// The identifier of the game type.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The module assigned to the selected game type.
    /// </returns>
    public ValueTask<IGameModule> GetModuleAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}
