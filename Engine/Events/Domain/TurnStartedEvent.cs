using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Events.Domain;

public sealed record class TurnStartedEvent(GameId GameId, EntityId PlayerId) : IGameEvent;