using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.Centuriin.Core.Common;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed record class CardDealtEvent(GameId GameId, CardId CardId, PlayerId NewOwnerId) : IGameEvent;