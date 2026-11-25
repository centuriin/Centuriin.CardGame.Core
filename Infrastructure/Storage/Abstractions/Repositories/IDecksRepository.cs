using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

/// <summary>
/// Retrieves card templates for a player's deck in a game type.
/// </summary>
public interface IDecksRepository
{
    /// <summary>
    /// Retrieves the card templates for the specified player's deck and game type.
    /// </summary>
    /// <param name="gameTypeId">
    /// The identifier of the game type.
    /// </param>
    /// <param name="playerId">
    /// The external player identifier.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The requested entity templates.
    /// </returns>
    public Task<IReadOnlyCollection<CardTemplate>> GetDeckTemplatesAsync(
        GameTypeId gameTypeId,
        PlayerId playerId,
        CancellationToken token);
}