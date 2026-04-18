using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed record class GameStartedEvent(GameId GameId) : IGameEvent;