using Centuriin.CardGame.Core.Common.ECS.Entities;

namespace Centuriin.CardGame.Core.Common.Events.Domain;

public sealed record class TurnStartedEvent(GameId GameId, EntityId PlayerId) : IGameEvent;