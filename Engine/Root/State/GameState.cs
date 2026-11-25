using System.ComponentModel;

using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Root.State;

/// <summary>
/// Stores mutable game status, entities, and the turn controller.
/// </summary>
public sealed class GameState : IGameState
{
    /// <inheritdoc/>
    public GameStatus Status { get; private set; } = GameStatus.Created;

    /// <inheritdoc/>
    public ITurnAutomat TurnAutomat { get; }

    /// <inheritdoc/>
    IReadOnlyTurnAutomat IReadOnlyGameState.TurnAutomat => TurnAutomat;

    /// <inheritdoc/>
    public IEntitiesCollection Entities { get; } = new EntitiesCollection();

    /// <inheritdoc/>
    IReadOnlyEntitiesCollection IReadOnlyGameState.Entities => Entities;

    /// <summary>
    /// Initializes a new <see cref="GameState"/> instance.
    /// </summary>
    /// <param name="turnAutomat">
    /// The controller used to manage the turn queue.
    /// </param>
    public GameState(ITurnAutomat turnAutomat)
    {
        ArgumentNullException.ThrowIfNull(turnAutomat);
        TurnAutomat = turnAutomat;
    }

    /// <inheritdoc/>
    public void ChangeStatus(GameStatus newStatus)
    {
        if (!Enum.IsDefined(newStatus))
        {
            throw new InvalidEnumArgumentException(
                nameof(newStatus),
                (int)newStatus,
                typeof(GameStatus));
        }

        Status = newStatus;
    }
}