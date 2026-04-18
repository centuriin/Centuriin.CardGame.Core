using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed record class TurnEndedEvent(GameId GameId, PlayerId PlayerId) : IGameEvent;