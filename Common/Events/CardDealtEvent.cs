using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed record class CardDealtEvent(GameId GameId, EntityId CardId, EntityId NewOwnerId) : IRandomEvent;