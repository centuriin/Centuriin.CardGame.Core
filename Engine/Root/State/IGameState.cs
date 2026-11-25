using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Root.State;

/// <summary>
/// Exposes game state that domain event consumers may modify.
/// </summary>
public interface IGameState : IReadOnlyGameState
{
    /// <inheritdoc cref="IReadOnlyGameState.TurnAutomat"/>
    /// <remarks>
    /// This view also exposes turn queue editing operations.
    /// </remarks>
    public new ITurnAutomat TurnAutomat { get; }

    /// <inheritdoc cref="IReadOnlyGameState.Entities"/>
    /// <remarks>
    /// This view also exposes entity and component editing operations.
    /// </remarks>
    public new IEntitiesCollection Entities { get; }

    /// <summary>
    /// Assigns a defined game lifecycle status.
    /// </summary>
    /// <param name="newStatus">
    /// The game status to assign.
    /// </param>
    public void ChangeStatus(GameStatus newStatus);
}
