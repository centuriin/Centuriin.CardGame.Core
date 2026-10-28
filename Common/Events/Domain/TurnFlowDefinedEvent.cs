using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common.Events.Domain;

public sealed record class TurnFlowDefinedEvent(
    GameId GameId,
    IReadOnlyCollection<EntityId> InitialPlayerTurnsOrder,
    bool IsCycled) : IGameEvent;