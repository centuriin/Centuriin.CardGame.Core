using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Events.Domain;

public sealed record class CardDealtEvent(
    GameId GameId,
    EntityId CardId,
    EntityId NewZoneId) : IGameEvent;