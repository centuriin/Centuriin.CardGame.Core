namespace Centuriin.CardGame.Core.Engine.Events.Domain;

/// <summary>
/// Triggers game-start systems on the authoritative game.
/// </summary>
/// <param name="GameId">
/// The identifier of the associated game.
/// </param>
public sealed record class GameStartedEvent(GameId GameId) : IGameEvent;