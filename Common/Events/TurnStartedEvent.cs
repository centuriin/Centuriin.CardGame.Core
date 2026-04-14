using Centuriin.CardGame.Core.Common.Entities.Players;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed record class TurnStartedEvent(GameId GameId, PlayerId PlayerId) : IGameEvent;