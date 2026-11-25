using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

/// <summary>
/// Retrieves entity templates for a game type.
/// </summary>
/// <typeparam name="TTemplate">
/// The entity template type.
/// </typeparam>
public interface ITemplatesRepository<TTemplate>
    where TTemplate : TemplateBase
{
    /// <summary>
    /// Retrieves the templates associated with a game type.
    /// </summary>
    /// <param name="gameTypeId">
    /// The identifier of the game type.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The requested entity templates.
    /// </returns>
    public Task<IReadOnlyCollection<TTemplate>> GetTemplatesAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}