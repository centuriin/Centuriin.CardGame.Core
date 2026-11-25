using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Events.Domain;

/// <summary>
/// Records the initial player turn order and whether it repeats.
/// </summary>
public sealed class TurnFlowDefinedEvent : IGameEvent
{
    /// <inheritdoc/>
    public GameId GameId { get; }

    /// <summary>
    /// Gets the initial player entity identifiers in turn order.
    /// </summary>
    public IReadOnlyCollection<EntityId> InitialPlayerTurnsOrder { get; }

    /// <summary>
    /// Gets whether the configured player order repeats when the queue is exhausted.
    /// </summary>
    public bool IsCycled { get; }

    /// <summary>
    /// Initializes a new <see cref="TurnFlowDefinedEvent"/> instance.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="initialPlayerTurnsOrder">
    /// The initial player entity identifiers in turn order.
    /// </param>
    /// <param name="isCycled">
    /// Whether the player turn order should repeat.
    /// </param>
    public TurnFlowDefinedEvent(
        GameId gameId,
        IReadOnlyCollection<EntityId> initialPlayerTurnsOrder,
        bool isCycled)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(initialPlayerTurnsOrder);

        if (initialPlayerTurnsOrder.Count == 0)
        {
            throw new ArgumentException(
                "Initial player turns order can't be empty.",
                nameof(initialPlayerTurnsOrder));
        }
        InitialPlayerTurnsOrder = initialPlayerTurnsOrder;

        IsCycled = isCycled;
    }
}