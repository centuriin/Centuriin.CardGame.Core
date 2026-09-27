using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Domain;

public sealed record class CardDealtEvent(
    GameId GameId, 
    EntityId CardId, 
    EntityId NewZoneId) : IGameEvent;