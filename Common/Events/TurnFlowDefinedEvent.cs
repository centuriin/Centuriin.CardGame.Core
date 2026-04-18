using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed record class TurnFlowDefinedEvent(
    GameId GameId,
    IReadOnlyCollection<PlayerId> InitialPlayerTrunsOrder,
    bool IsCycled) : IGameEvent;