namespace Centuriin.CardGame.Core.Common.Events.Domain;

public sealed record class GameStartedEvent(GameId GameId) : IGameEvent;