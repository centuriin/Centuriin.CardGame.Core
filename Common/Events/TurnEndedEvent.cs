using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed record class TurnEndedEvent(GameId GameId, EntityId PlayerId) : IGameEvent;