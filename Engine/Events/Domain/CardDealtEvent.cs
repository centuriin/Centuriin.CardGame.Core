using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Events.Domain;

/// <summary>
/// Records the card and destination zone selected by a dealing system.
/// </summary>
/// <param name="GameId">
/// The identifier of the associated game.
/// </param>
/// <param name="CardId">
/// The identifier of the dealt card.
/// </param>
/// <param name="NewZoneId">
/// The destination zone identifier.
/// </param>
public sealed record class CardDealtEvent(
    GameId GameId,
    EntityId CardId,
    EntityId NewZoneId) : IGameEvent;