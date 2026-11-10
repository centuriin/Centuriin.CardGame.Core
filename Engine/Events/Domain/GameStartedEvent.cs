namespace Centuriin.CardGame.Core.Engine.Events.Domain;

public sealed record class GameStartedEvent(GameId GameId) : IGameEvent;