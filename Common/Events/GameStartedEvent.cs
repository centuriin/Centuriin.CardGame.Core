using Centuriin.Centuriin.Core.Common;

namespace Centuriin.CardGame.Core.Common.Events;

public sealed record class GameStartedEvent(GameId GameId) : IGameEvent;