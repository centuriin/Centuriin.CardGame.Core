using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Application.Initialization;

/// <summary>
/// Creates entities using the game description and previously initialized entities.
/// </summary>
public interface IEntityInitializer
{
    /// <summary>
    /// Creates the entities contributed by this initialization step.
    /// </summary>
    /// <param name="description">
    /// The game description containing the type, settings, and players.
    /// </param>
    /// <param name="context">
    /// The initial state populated by preceding initializers.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// The entities to add to the initial state.
    /// </returns>
    public Task<IReadOnlyCollection<EntityBase>> InitializeAsync(
        GameDescription description,
        IReadOnlyInitialStateContext context,
        CancellationToken token);
}
