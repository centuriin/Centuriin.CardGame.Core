using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Events.Domain;

public sealed record class TurnEndedEvent(GameId GameId, EntityId PlayerId) : IGameEvent;