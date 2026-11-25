using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Events.Domain;

/// <summary>
/// Records the end of a player's turn.
/// </summary>
/// <param name="GameId">
/// The identifier of the associated game.
/// </param>
/// <param name="PlayerId">
/// The identifier of the player entity whose turn changed.
/// </param>
public sealed record class TurnEndedEvent(GameId GameId, EntityId PlayerId) : IGameEvent;