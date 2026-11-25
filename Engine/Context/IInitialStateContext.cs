using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Context;

/// <summary>
/// Provides mutable entities during game initialization.
/// </summary>
public interface IInitialStateContext : IReadOnlyInitialStateContext
{
    /// <inheritdoc cref="IReadOnlyInitialStateContext.Entities"/>
    /// <remarks>
    /// This view also exposes entity and component editing operations.
    /// </remarks>
    public new IEntitiesCollection Entities { get; }

    /// <inheritdoc/>
    IReadOnlyEntitiesCollection IReadOnlyInitialStateContext.Entities => Entities;
}
