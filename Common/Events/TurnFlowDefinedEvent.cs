using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed record class TurnFlowDefinedEvent(
    GameId GameId,
    IReadOnlyCollection<EntityId> InitialPlayerTrunsOrder,
    bool IsCycled) : IGameEvent;