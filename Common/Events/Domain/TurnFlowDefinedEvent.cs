using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common.Events.Domain;

public sealed class TurnFlowDefinedEvent : IGameEvent
{
    public GameId GameId { get; }

    public IReadOnlyCollection<EntityId> InitialPlayerTurnsOrder { get; }

    public bool IsCycled { get; }

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